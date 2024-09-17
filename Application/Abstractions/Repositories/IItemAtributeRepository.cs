using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IItemAtributeRepository
    {
        public Task<ItemAttribute?> GetItemAtribute(int attributeId);
    }
}