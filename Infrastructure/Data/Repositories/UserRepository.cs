using Application.Abstractions.Repositories;
using Application.Features.ItemFamilies.Queries.GetAllItemFamilies;
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
                User? user = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @id", new { id });

                if (user is null)
                {
                    return null;
                }

                user.UserRole = await GetUserRoleWithDepartments(connection, user);

                return user;
            }
        }

        public async Task<User?> GetUserByName(string name)
        {
            using (SqlConnection connection = new(connectionString))
            {
                User? user = await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Name = @name", new { name });

                if (user is null)
                {
                    return null;
                }

                user.UserRole = await GetUserRoleWithDepartments(connection, user);

                return user;
            }
        }

        private async Task<UserRole?> GetUserRoleWithDepartments(SqlConnection connection, User user)
        {
            string getUserRoleSql = "SELECT * FROM Roles WHERE Id = @userRoleId";
            UserRole? role = await connection.QueryFirstOrDefaultAsync<UserRole>(getUserRoleSql, new { userRoleId = user.RoleId });

            if (role is null)
            {
                return null;
            }

            string getRoleDepartments =
            @"SELECT * 
            FROM Departments 
            WHERE Id IN (
                SELECT DepartmentId 
                FROM Roles_Department 
                WHERE RoleId = @userRoleId)";

            IEnumerable<Department> roleDepartments = await connection.QueryAsync<Department>(getRoleDepartments, new { userRoleId = role.Id });

            role.Departments = roleDepartments.ToList();

            return role;
        }
    }
}