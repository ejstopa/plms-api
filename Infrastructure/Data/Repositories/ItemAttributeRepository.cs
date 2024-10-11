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

        public async Task<ItemAttribute?> GetItemAtributeById(int attributeId)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = "SELECT * FROM ItemAttributes WHERE Id = @attributeId";

                return await connection.QueryFirstOrDefaultAsync<ItemAttribute>(sql, new { attributeId });
            }
        }

        public async Task<List<ItemAttribute>> GetItemsAttributesByFamily(int familyId)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string selectcAttributesSql =
                @"SELECT * 
                FROM ItemAttributes 
                WHERE Id IN (
                    SELECT ItemAttributeId 
                    FROM WorkflowSteps_ItemAttributes 
                    WHERE ItemFamilyId = @familyId)";
                List<ItemAttribute> itemAttributes = (await connection.QueryAsync<ItemAttribute>(
                    selectcAttributesSql, new { familyId })).ToList();

                string selectOptionsSql = "SELECT * FROM ItemAttributeOptions WHERE ItemAttributeId IN @attributesId";
                IEnumerable<ItemAttributeOption> options = await connection.QueryAsync<ItemAttributeOption>(
                    selectOptionsSql, new {attributesId = itemAttributes.Select(attribute => attribute.Id)});

                itemAttributes.ForEach(
                    attribute => attribute.Options = options.Where(option => option.ItemAttributeId == attribute.Id).ToList());

                return itemAttributes;
            }
        }
    }
}