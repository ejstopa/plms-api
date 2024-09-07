using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IItemNameReservationRepository
    {
        public Task<ItemNameReservation?> CreateItemNameReservation(string itemFamily, int userId);
        public Task<ItemNameReservation?> GetItemNameReservationById(int id);
        public Task<ItemNameReservation?> GetItemNameReservationByName(string name);
    }
    
}