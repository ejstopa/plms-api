using Application.Abstractions.Repositories;
using Dapper;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.Repositories
{
    public class ModelRepository : IModelRepository
    {
        readonly string connectionString;

        public ModelRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<Model> CreateModel(Model model)
        {
            using (SqlConnection connection = new(connectionString))
            {
                string sql = @"INSERT INTO Models Values(
                    @FileName, @CommonName, @Revision, @Type, 
                    @Description, @Stauts, @LibraryFolder, @CreatedBy,
                    @CheckedOutBy, @LastModifiedBy, @CreatedAt, @LastModifiedAt, @CheckedOutAt);
                    SELECT CAST(SCOPE_IDENTITY() as INT)";

                int id = await connection.ExecuteScalarAsync<int>(sql, model);

                return await connection.QueryFirstAsync<Model>("SELECT * FROM Models WHERE Id = @id", new { id });
            }
        }

        public async Task<IEnumerable<Model>> GetUserCheckedoutModels(int userId)
        {
            using (SqlConnection connection = new(connectionString))
            {
                string sql = @"SELECT * FROM Models WHERE CheckedOutBy = @userId";

                return await connection.QueryAsync<Model>(sql, new { userId });
            }
        }

        public async Task<List<Model>> GetModelsByFamily(string family)
        {
            using (SqlConnection connection = new(connectionString))
            {
                string sql = @"SELECT T.* 
                            FROM Items 
                            RIGHT JOIN (
                                SELECT *, ROW_NUMBER() OVER (PARTITION BY 
                                Name, Type ORDER BY Version DESC) AS row_number FROM Models) AS T 
                            ON T.ItemId = Items.Id
                            WHERE T.row_number = 1 AND Items.Family = @family";

                var models = await connection.QueryAsync<Model>(sql, new { family });

                return models.ToList();
            }
        }

        public async Task<Model?> GetModelById(int id)
        {
            using (SqlConnection connection = new(connectionString))
            {
                string sql = "SELECT *FROM Models WHERE Id = @id";

                var model = await connection.QueryFirstOrDefaultAsync<Model>(sql, new { Id = id });

                return model;
            }
        }

        public async Task<List<Model>> GetLatestModelsByItem(int itemId)
        {
            using (SqlConnection connection = new(connectionString))
            {
                 string sql = @"SELECT *
                                FROM (
                                    SELECT *, ROW_NUMBER() OVER (PARTITION BY
                                    Name, Type ORDER BY Version DESC) AS row_number
                                    FROM Models
                                ) AS t
                                WHERE t.row_number = 1 AND ItemId = @itemId";

                var models = await connection.QueryAsync<Model>(sql, new { itemId });

                return models.ToList();
            }
        }
    }
}