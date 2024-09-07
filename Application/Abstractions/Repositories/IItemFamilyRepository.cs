using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IItemFamilyRepository
    {
        public Task<List<ItemFamily>> GetAllItemFamilies();
        public Task<ItemFamily?> GetItemFamilyByName(string familyName);
    }
}