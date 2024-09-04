
using Application.Abstractions.FileSystem;
using Application.Abstractions.Repositories;
using Domain.Entities;
using Domain.Enums;
using Domain.Services;
using Infrastructure.Data.Repositories;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.FileSystem
{
    public class UserWorkspaceFilesService : IUserWorkspaceFIlesService
    {
        private readonly IConfiguration _configuration;
        private readonly IModelRepository _modelRepository;
        private readonly IModelRevisionService _modelRevisionService;

        public UserWorkspaceFilesService(
            IConfiguration configuration,
            IModelRepository modelRepository,
            IModelRevisionService modelRevisionService)
        {
            _configuration = configuration;
            _modelRepository = modelRepository;
            _modelRevisionService = modelRevisionService;
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

        public  List<UserFile> GetUserUserWorkspaceFiles(User user, List<string>? filterExtensions = null)
        {
            string userWorkspaceDir = GetUserWorkspaceDirectory(user);
            List<string> workspaceFilePaths = Directory.GetFiles(userWorkspaceDir).ToList();

            List<UserFile> userFiles = [];

            foreach (string filePath in workspaceFilePaths)
            {
                string fileNameWithExtension = filePath[(filePath.LastIndexOf('\\') + 1)..];
                string fileExtensionWithVersion = fileNameWithExtension[fileNameWithExtension.IndexOf('.')..];

                string fileName = fileNameWithExtension[..fileNameWithExtension.IndexOf('.')];
                string fileExtension = fileExtensionWithVersion[1 ..].Contains('.')? 
                fileExtensionWithVersion[.. fileExtensionWithVersion.LastIndexOf('.')] :
                fileExtensionWithVersion;

                if (filterExtensions != null && filterExtensions.Contains(fileExtension))
                userFiles.Add(new UserFile()
                {
                    Name = fileName,
                    Extension = fileExtension,
                    FullPath = filePath,
                    LastModifiedAt = File.GetLastWriteTime(filePath),
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