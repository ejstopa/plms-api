using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Application.Abstractions.Repositories;
using Dapper;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.Repositories
{
    public class ItemRepository : IItemRepository
    {
        readonly string _connectionString;

        public ItemRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<Item?> GetItemById(int itemId)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = "SELECT * FROM Items WHERE Id = @id";

                return await connection.QueryFirstOrDefaultAsync<Item>(sql, new { id = itemId });
            }
        }

        public async Task<bool> ToggleItemCheckout(int itemId, int userId, bool checkedOut)
        {
            int checkedOutId = checkedOut? userId : 0;

            using (SqlConnection connection = new(_connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        string updateItemsSql = "UPDATE Items SET CheckedOutBy = @checkedOutId WHERE Id = @itemId";
                        int rowsUpdated = await connection.ExecuteAsync(updateItemsSql, new { checkedOutId, itemId }, transaction);

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

           
    }
}