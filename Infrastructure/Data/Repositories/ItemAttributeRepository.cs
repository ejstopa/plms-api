using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.Repositories;
using Dapper;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.Repositories
{
    public class ItemAttributeRepository : IItemAtributeRepository
    {
        private readonly string _connectionString;

        public ItemAttributeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<ItemAtribute?> GetItemAtribute(int attributeId)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = "SELECT * FROM ItemAttributes WHERE Id = @attributeId";

                return await connection.QueryFirstOrDefaultAsync<ItemAtribute>(sql, new {attributeId});
            }
        }
    }
}