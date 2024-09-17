using System.Diagnostics.CodeAnalysis;
using Application.Abstractions.FileSystem;
using Application.Abstractions.Repositories;
using Dapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly string _connectionString;
        private readonly IUserWorkspaceFIlesService _userWorkspaceFIlesService;
        private readonly IUserWorkflowFilesService _userWorkflowFilesService;
        private readonly IModelRevisionService _modelRevisionService;

        public ItemRepository(
            IConfiguration configuration,
            IUserWorkspaceFIlesService userWorkspaceFIlesService,
            IUserWorkflowFilesService userWorkflowFilesService,
            IModelRevisionService modelRevisionService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
            _userWorkspaceFIlesService = userWorkspaceFIlesService;
            _userWorkflowFilesService = userWorkflowFilesService;
            _modelRevisionService = modelRevisionService;
        }

        public async Task<Item?> GetItemById(int itemId)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = "SELECT * FROM Items WHERE Id = @id";

                return await connection.QueryFirstOrDefaultAsync<Item>(sql, new { id = itemId });
            }
        }

        public async Task<Item?> CreateItem(Item item)
        {
            using (SqlConnection connecton = new(_connectionString))
            {
                string sql =
                @"INSERT INTO Items VALUES (
                    @Name, @Revision, @Version, @Description, 
                    @Family, @Status, @CreatedBy, @CreatedAt, 
                    @LastModifiedBy, @LastModifiedAt, @CheckedOutBy, 
                    @FamilyId )
                SELECT CAST(SCOPE_IDENTITY() as INT)";

                int id = await connecton.ExecuteScalarAsync<int>(sql, item);

                return await connecton.QueryFirstOrDefaultAsync<Item>("SELECT * FROM Items WHERE Id = @id", new { id });

            }

        }

        public async Task<Item?> GetLatestItemByName(string itemName, bool getModels = false)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = "SELECT TOP 1 * FROM Items WHERE Name = @itemName ORDER BY Id DESC";

                Item? item = await connection.QueryFirstOrDefaultAsync<Item>(sql, new { itemName });

                if (item is null)
                {
                    return null;
                }

                List<Item> items = [item];

                if (getModels)
                {
                    List<Model> models = await GetItemsModels(items.ToList());
                    List<Item> itemsWithModels = AddItemsModels(items.ToList(), models);
                }

                return items[0];
            }
        }

        public async Task<List<Item>> GetUserCheckedOutItems(int userId)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = "SELECT * FROM Items WHERE CheckedOutBy = @userId";

                IEnumerable<Item> items = await connection.QueryAsync<Item>(sql, new { userId });

                return items.ToList();
            }
        }

        public async Task<List<Item>> GetItemsByFamily(string family)
        {
            IEnumerable<Item> items;

            using (SqlConnection connection = new(_connectionString))
            {
                string sql = @"SELECT *
                                FROM (
                                    SELECT *, ROW_NUMBER() OVER (PARTITION BY
                                    Name ORDER BY Version DESC) AS row_number
                                    FROM Items
                                ) AS t
                                WHERE t.row_number = 1 AND Family = @family";

                items = await connection.QueryAsync<Item>(sql, new { family });
            }

            List<Model> models = await GetItemsModels(items.ToList());
            List<Item> itemsWithModels = AddItemsModels(items.ToList(), models);

            return itemsWithModels;
        }

        public async Task<List<Item>> GetItemsByUserWorkspace(User user)
        {
            List<Item> checkedOutItemsWithModels = await GetCheckedOutItemsWithModels(user);
            List<Item> newItemsWithModels = GetNewUserWorkspaceItems(user, checkedOutItemsWithModels);

            List<Item> items = [.. checkedOutItemsWithModels, .. newItemsWithModels];

            return items;
        }

        public async Task<bool> ToggleItemCheckout(int itemId, int userId, bool checkOut)
        {
            int checkedOutId = checkOut ? userId : 0;
            string itemStatus = checkOut ? ItemStatus.checkedOut.ToString() : ItemStatus.released.ToString();

            using (SqlConnection connection = new(_connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        string updateItemsSql = "UPDATE Items SET CheckedOutBy = @checkedOutId, Status = @itemStatus WHERE Id = @itemId";
                        int rowsUpdated = await connection.ExecuteAsync(updateItemsSql, new { checkedOutId, itemStatus, itemId }, transaction);

                        if (rowsUpdated != 1)
                        {
                            await transaction.RollbackAsync();
                            return false;
                        }

                        string getModelsSql = @"SELECT *
                                            FROM (
                                                SELECT *, ROW_NUMBER() OVER (PARTITION BY
                                                Name, Type ORDER BY Version DESC) AS row_number
                                                FROM Models
                                            ) AS t
                                            WHERE t.row_number = 1 AND ItemId = @itemId";
                        IEnumerable<Model> models = await connection.QueryAsync<Model>(getModelsSql, new { itemId }, transaction);

                        List<int> modelIds = models.Select(model => model.Id).ToList();

                        string updateModelsSql = "UPDATE Models SET CheckedOutBy = @checkedOutId WHERE Id IN @modelIds";
                        int modelRowsUpdated = await connection.ExecuteAsync(updateModelsSql, new { checkedOutId, @modelIds = modelIds.ToArray() }, transaction);

                        await transaction.CommitAsync();
                        return true;
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        return false;
                    }
                }
            }
        }

        public async Task<Item?> SetItemStatus(int itemId, ItemStatus itemStatus)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = @"UPDATE Items 
                            SET Status = @itemStatus 
                            WHERE Id = @itemId
                            SELECT CAST(SCOPE_IDENTITY() as INT)";
                int rowsEdited = await connection.ExecuteAsync(sql, new { itemId, itemStatus = itemStatus.ToString() });

                if (rowsEdited == 0)
                {
                    return null;
                }
                
                return await connection.QueryFirstAsync<Item>("SELECT * FROM Items WHERE Id = @itemId", new { itemId });
            }
        }
       
        private async Task<List<Item>> GetCheckedOutItemsWithModels(User user)
        {
            List<UserFile> files = _userWorkspaceFIlesService.GetUserUserWorkspaceFiles(user, [".prt", ".asm", ".drw"]);

            List<Item> checkedOutItems = [.. (await GetUserCheckedOutItems(user.Id)).OrderBy(Item => Item.Name)];
            List<Item> checkedOutItemsFilterd = checkedOutItems.Where(item => files.Select(file => file.Name).Contains(item.Name)).ToList();

            checkedOutItemsFilterd.ForEach(item =>
            {
                item.Revision = _modelRevisionService.IncrementRevision(item.Revision);
                item.Status = ItemStatus.checkedOut.ToString();
            });
            List<Model> checkedOutItemsModels = [.. (await GetItemsModels(checkedOutItemsFilterd)).OrderBy(model => model.Type != ".drw" ? 0 : 1)];

            string workspaceDirectory = _userWorkspaceFIlesService.GetUserWorkspaceDirectory(user);

            checkedOutItemsModels.ForEach(model =>
            {
                string libraryDirectory = model.FilePath[..model.FilePath.LastIndexOf('\\')];
                model.FilePath = model.FilePath.Replace(libraryDirectory, workspaceDirectory);
            });

            List<Item> checkedOutItemsWithModels = AddItemsModels(checkedOutItemsFilterd, checkedOutItemsModels);

            return [.. checkedOutItemsWithModels.OrderBy(item => item.Name)];
        }

        private List<Item> GetNewUserWorkspaceItems(User user, List<Item> checkedOutItems)
        {
            List<UserFile> files = _userWorkspaceFIlesService.GetUserUserWorkspaceFiles(user, [".prt", ".asm", ".drw"]);
            List<UserFile> newFiles = files.Where(file => !checkedOutItems.Where(item => item.Name == file.Name).Any()).ToList();

            List<Item> newItems = [];

            newFiles.OrderBy(file => file.Extension != ".drw" ? 0 : 1).GroupBy(file => file.Name).ToList()
            .ForEach(fileGroup =>
            {
                List<Model> models = [];
                fileGroup.AsList().ForEach(file => models.Add(new Model
                {
                    Name = file.Name,
                    Type = file.Extension,
                    FilePath = file.FullPath,
                    LastModifiedAt = file.LastModifiedAt
                }));

                int familyResult = 0;
                bool isFamilyNumber = false;

                try
                {
                    isFamilyNumber = int.TryParse(fileGroup.AsList()[0].Name[..4], out familyResult);
                }
                catch { }

                newItems.Add(new Item
                {
                    Name = fileGroup.AsList()[0].Name,
                    Status = ItemStatus.newItem.ToString(),
                    Family = isFamilyNumber ? fileGroup.AsList()[0].Name[..4] : "",
                    Models = models

                });
            });

            return newItems;
        }

        private async Task<List<Model>> GetItemsModels(List<Item> items)
        {
            int?[] itemIds = items.Select(item =>
            {
                return item.Id != null ? item.Id : 0;
            }).ToArray();

            using (SqlConnection connection = new(_connectionString))
            {
                string sql = @"SELECT *
                                FROM (
                                    SELECT *, ROW_NUMBER() OVER (PARTITION BY
                                    Name, Type ORDER BY Version DESC) AS row_number
                                    FROM Models
                                ) AS t
                                WHERE t.row_number = 1 AND ItemId IN @itemIds";

                IEnumerable<Model> models = await connection.QueryAsync<Model>(sql, new { itemIds });

                return models.ToList();
            }
        }

        private List<Item> AddItemsModels(List<Item> items, List<Model> models)
        {
            List<Item> itemsWithModels = [.. items];

            items.ForEach(item => item.Models = models.Where(model => model.ItemId == item.Id).ToList());

            return itemsWithModels;
        }



    }
}