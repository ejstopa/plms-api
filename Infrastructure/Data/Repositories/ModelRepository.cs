using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.Repositories;
using Dapper;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
                string sql = @"SELECT * FROM Models WHERE CheckedOutBy = @id";

                return await  connection.QueryAsync<Model>(sql, new {id = userId});
            }
        }
    }
}