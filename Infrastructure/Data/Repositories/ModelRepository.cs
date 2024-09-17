using Application.Abstractions.Repositories;
using Dapper;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.Repositories
{
    public class ModelRepository : IModelRepository
    {
        private readonly string connectionString;

        public ModelRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<Model?> CreateModel(Model model)
        {
            using (SqlConnection connection = new(connectionString))
            {
                string sql = 
                @"INSERT INTO Models Values(
                    @Name, @CommonName, @Type, @Version, 
                    @Revision, @FilePath, @ItemId, @CreatedBy, 
                    @CreatedAt, @CheckedOutBy, @LastModifiedBy, @LastModifiedAt);
                SELECT CAST(SCOPE_IDENTITY() as INT)";

                int id = await connection.ExecuteScalarAsync<int>(sql, model);

                return await connection.QueryFirstOrDefaultAsync<Model>("SELECT * FROM Models WHERE Id = @id", new { id });
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