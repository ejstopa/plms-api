using Application.Abstractions.Repositories;
using Dapper;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.Repositories
{
    public class ItemFamilyRepository : IItemFamilyRepository
    {
        private readonly string _connectionString;

        public ItemFamilyRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<ItemFamily>> GetAllItemFamilies()
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = "SELECT * FROM ItemFamilies";
                IEnumerable<ItemFamily> itemFamilies = await connection.QueryAsync<ItemFamily>(sql);

                return itemFamilies.ToList();
            }
        }

        public async Task<ItemFamily?> GetItemFamilyByName(string familyName)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = "SELECT * FROM ItemFamilies WHERE Name = @familyName";

                return await connection.QueryFirstOrDefaultAsync<ItemFamily>(sql, new { familyName });
            }
        }

        public async Task<ItemFamily?> GetItemFamilyById(int familyId)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = "SELECT * FROM ItemFamilies WHERE Id = @familyId";

                return await connection.QueryFirstOrDefaultAsync<ItemFamily>(sql, new { familyId });
            }
        }
    }
}