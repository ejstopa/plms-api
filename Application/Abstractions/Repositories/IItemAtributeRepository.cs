using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IItemAtributeRepository
    {
        public Task<ItemAttribute?> GetItemAtributeById(int attributeId);
        public Task<List<ItemAttribute>> GetItemsAttributesByFamily(int familyId);
    }
}