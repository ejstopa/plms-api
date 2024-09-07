
using Application.Abstractions.Repositories;
using Dapper;
using Domain.Entities;
using Domain.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data.Repositories
{
    public class ItemNameReservationRepository : IItemNameReservationRepository
    {
        private readonly string _connectionString;
        private readonly IItemNameReservationService _itemNameReservationService;

        public ItemNameReservationRepository(IConfiguration configuration,
        IItemNameReservationService itemNameReservationService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
            _itemNameReservationService = itemNameReservationService;
        }

        public async Task<ItemNameReservation?> CreateItemNameReservation(string itemFamily, int userId)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                ItemNameReservation? lastReservation = await GetLatestReservationByFamily(connection, itemFamily);
                string? newItemName = "";

                newItemName = lastReservation is null ?
                    $"{itemFamily}0001" :
                    _itemNameReservationService.IncrementItemName(lastReservation.ItemName);

                if (newItemName is null)
                {
                    return null;
                }

                string sql = @"INSERT INTO ItemNameReservations (UserId, ItemName, ItemFamily)
                            VALUES (@userId, @newItemName, @itemFamily)
                            SELECT CAST(SCOPE_IDENTITY() as INT)";

                int id = await connection.ExecuteScalarAsync<int>(sql, new { userId, newItemName, itemFamily });

                return await GetItemNameReservationById(id);
            }
        }

        public async Task<ItemNameReservation?> GetItemNameReservationById(int id)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = "SELECT * FROM ItemNameReservations WHERE Id = @id";

                return await connection.QueryFirstOrDefaultAsync<ItemNameReservation>(sql, new { id });
            }
        }

        public async Task<ItemNameReservation?> GetItemNameReservationByName(string name)
        {
            using (SqlConnection connection = new(_connectionString))
            {
                string sql = "SELECT * FROM ItemNameReservations WHERE ItemName = @name";

                return await connection.QueryFirstOrDefaultAsync<ItemNameReservation>(sql, new {name});
            }
        }

        private async Task<ItemNameReservation?> GetLatestReservationByFamily(SqlConnection connection, string itemFamily)
        {
            string sql = @"SELECT  TOP 1 * FROM ItemNameReservations 
                        WHERE ItemFamily = @itemFamily 
                        ORDER BY ItemName DESC";

            ItemNameReservation? reservation = await connection.QueryFirstOrDefaultAsync<ItemNameReservation>(
                sql, new { itemFamily });

            return reservation;
        }


    }
}