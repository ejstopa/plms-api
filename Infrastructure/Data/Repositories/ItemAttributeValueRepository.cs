using Application.Abstractions.Repositories;
using Domain.Entities;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.Repositories
{
    public class ItemAttributeValueRepository : IItemAtributeValueRepository
    {
        private readonly string _connectionString;

        public ItemAttributeValueRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<ItemAttributeValue?> GetItemAttributeValue(int itemId, int attributeId)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = "SELECT * FROM ItemAttributeValues WHERE ItemId = @itemId AND ItemAttributeId = @attributeId";

                ItemAttributeValue? attributeValue = await connection.QueryFirstOrDefaultAsync<ItemAttributeValue>(
                    sql, new { itemId, attributeId });

                return attributeValue;
            };
        }

        public async Task<ItemAttributeValue?> CreateItemAttributeValue(int itemId, int attributeId, string value)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql =
                @"INSERT INTO ItemAttributeValues (ItemId, ItemAttributeId, ItemAttributeValueString) 
                VALUES (@itemId, @attributeId, @value) 
                SELECT CAST(SCOPE_IDENTITY() as INT)";

                int id = await connection.ExecuteScalarAsync<int>(sql, new { itemId, attributeId, value });

                return await connection.QueryFirstOrDefaultAsync<ItemAttributeValue>(
                    "SELECT * FROM ItemAttributeValues WHERE Id = @id", new { id });
            }
        }

        public async Task<ItemAttributeValue?> CreateItemAttributeValue(int itemId, int attributeId, double value)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql =
                @"INSERT INTO ItemAttributeValues (ItemId, ItemAttributeId, ItemAttributeValueNumber) 
                VALUES (@itemId, @attributeId, @value) 
                SELECT CAST(SCOPE_IDENTITY() as INT)";

                int id = await connection.ExecuteScalarAsync<int>(sql, new { itemId, attributeId, value });

                return await connection.QueryFirstOrDefaultAsync<ItemAttributeValue>(
                    "SELECT * FROM ItemAttributeValues WHERE Id = @id", new { id });
            }
        }

        public async Task<ItemAttributeValue?> UpdateItemAttributeValue(int itemId, int attributeId, string value)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql =
                @"UPDATE ItemAttributeValues 
                SET ItemAttributeValueString = @value 
                WHERE ItemId = @itemId AND ItemAttributeId = @attributeId
                SELECT CAST(SCOPE_IDENTITY() as INT)";

                await connection.ExecuteAsync(sql, new { value, itemId, attributeId });

                string selectWorkflowValueSql =
                @"SELECT * FROM ItemAttributeValues 
                WHERE ItemId = @itemId AND ItemAttributeId = @attributeId";

                return await connection.QueryFirstOrDefaultAsync<ItemAttributeValue>(selectWorkflowValueSql, new { value, itemId, attributeId });
            }
        }

        public async Task<ItemAttributeValue?> UpdateItemAttributeValue(int itemId, int attributeId, double value)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql =
                @"UPDATE ItemAttributeValues 
                SET ItemAttributeValueNumber = @value 
                WHERE ItemId = @itemId AND ItemAttributeId = @attributeId
                SELECT CAST(SCOPE_IDENTITY() as INT)";

                await connection.ExecuteAsync(sql, new { value, itemId, attributeId });

                string selectWorkflowValueSql =
                @"SELECT * FROM ItemAttributeValues 
                WHERE ItemId = @itemId AND ItemAttributeId = @attributeId";

                return await connection.QueryFirstOrDefaultAsync<ItemAttributeValue>(selectWorkflowValueSql, new { value, itemId, attributeId }); ;
            }
        }
    }
}