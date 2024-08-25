using Application.Abstractions.Repositories;
using Dapper;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        readonly string connectionString;

        public UserRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<User?> GetUserById(int id)
        {
            using (SqlConnection connection = new(connectionString))
            {
               return await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @id", new { id });
            }
        }

        public async Task<User?> GetUserByName(string name)
        {
            using (SqlConnection connection = new(connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Name = @name", new{ name });
            }
        }
    }
}