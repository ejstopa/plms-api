using Domain.Entities;

namespace Application.Abstractions.FileSystem
{
    public interface IUserWorkspaceFIlesService
    {
        public string GetUserWorkspaceDirectory(User user);

        public Task<List<UserFile>> GetUserUserWorkspaceFiles(User user);

        public Boolean DeleteUserWorkspaceFile(string filePath);
      
    }
}