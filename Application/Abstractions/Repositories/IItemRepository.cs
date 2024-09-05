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
        public Task<Item?> GetItemByName(string itemName);
        public Task<List<Item>> GetItemsByFamily(string family);
        public Task<List<Item>> GetUserCheckedOutItems(int userId);
        public Task<List<Item>> GetItemsByUserWorkspace(User user);
        public Task<bool> ToggleItemCheckout(int itemId, int userId, bool checkedOut);
        
    }
}