using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IItemAtributeValueRepository
    {
        public Task<ItemAttributeValue?> GetItemAttributeValue(int itemId, int attributeId);
        public Task<ItemAttributeValue?> CreateItemAttributeValue(int itemId, int attributeId, string value);
        public Task<ItemAttributeValue?> CreateItemAttributeValue(int itemId, int attributeId, double value);
        public Task<ItemAttributeValue?> UpdateItemAttributeValue(int itemId, int attributeId, string value);
        public Task<ItemAttributeValue?> UpdateItemAttributeValue(int itemId, int attributeId, double value);
    }
}