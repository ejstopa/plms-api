
using Application.Abstractions.FileSystem;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Infrastructure.Data.Repositories;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.FileSystem
{
    public class UserWorkspaceFilesService : IUserWorkspaceFIlesService
    {
        private readonly IConfiguration _configuration;
        private readonly IModelRepository _modelRepository;

        public UserWorkspaceFilesService(IConfiguration configuration, IModelRepository modelRepository)
        {
            _configuration = configuration;
            _modelRepository = modelRepository;
        }

        public string GetUserWorkspaceDirectory(User user)
        {
            if (user.WindowsUser is null)
            {
                throw new Exception("O perfil do usuário não contém definição do Windows User");
            }

            string workspaceDir = _configuration.GetSection("BaseDirectories")["UserWorkspaceBaseDir"]!
            + "/" + user.WindowsUser;

            return workspaceDir;
        }


        public async Task<List<UserFile>> GetUserUserWorkspaceFiles(User user)
        {
            string userWorkspaceDir = GetUserWorkspaceDirectory(user);
            List<string> workspaceFilePaths = Directory.GetFiles(userWorkspaceDir).ToList();

            IEnumerable<Model> userCheckedoutModels = await _modelRepository.GetUserCheckedoutModels(user.Id);

            List<UserFile> userFiles = [];

            foreach (string filePath in workspaceFilePaths)
            {
                string fileNameWithExtension = filePath[(filePath.LastIndexOf('\\') + 1)..];
                string fileExtensionWithVersion = fileNameWithExtension[fileNameWithExtension.IndexOf('.')..];

                string fileName = fileNameWithExtension[..fileNameWithExtension.IndexOf('.')];
                string fileExtension = fileExtensionWithVersion[1 ..].Contains('.')? 
                fileExtensionWithVersion[.. fileExtensionWithVersion.LastIndexOf('.')] :
                fileExtensionWithVersion;

                userFiles.Add(new UserFile()
                {
                    Name = fileName,
                    Extension = fileExtension,
                    FullPath = filePath,
                    LastModifiedAt = File.GetLastWriteTime(filePath),
                    IsRevision = userCheckedoutModels.Where(model => model?.Name == fileName && model.Type == fileExtension).Any()
                });
            }

            return userFiles;
        }

        public bool DeleteUserWorkspaceFile(string filePath)
        {
            try
            {
                File.Delete(filePath);
            }
            catch (Exception)
            {
                throw;
            }

           return true;
        }
    }
}