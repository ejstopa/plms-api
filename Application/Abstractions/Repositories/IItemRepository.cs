using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IItemRepository
    {
        public Task<Item?> GetItemById(int itemId);
        public Task<bool> ToggleItemCheckout(int itemId, int userId, bool checkedOut);
        
    }
}