using Domain.Entities;

namespace Application.Abstractions.FileSystem
{
    public interface IUserWorkspaceFIlesService
    {
        public string GetUserWorkspaceDirectory(User user);

        public List<UserFile> GetUserUserWorkspaceFiles(User user, List<string>? filterExtensions = null);

        public Boolean DeleteUserWorkspaceFile(string filePath);
      
    }
}