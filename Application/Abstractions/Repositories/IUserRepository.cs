using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IUserRepository
    {
        public Task<User?> GetUserById(int id);
        public Task<User?> GetUserByName(string name);
    }
}