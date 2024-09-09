using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.FileSystem;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.FileSystem
{
    public class UserWorkflowFilesService : IUserWorkflowFilesService
    {
        private readonly IConfiguration _configuration;
        public UserWorkflowFilesService(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        public string GetUserWorkflowsDirectory(User user)
        {
            if (user.WindowsUser is null)
            {
                throw new Exception("O perfil do usuário não contém definição do Windows User");
            }

            string userWorkflowsDir = _configuration.GetSection("BaseDirectories")["WorkflowsBaseDir"]!
            + "/" + user.WindowsUser;

            return userWorkflowsDir;
        }

        public List<UserFile> GetUserUserWorkflowFiles(User user, List<string>? filterExtensions)
        {
            string userWorkflowsDir = GetUserWorkflowsDirectory(user);
            List<string> workflowsFilePaths = [.. Directory.GetFiles(userWorkflowsDir)];

            List<UserFile> userFiles = [];

            foreach (string filePath in workflowsFilePaths)
            {
                string fileNameWithExtension = filePath[(filePath.LastIndexOf('\\') + 1)..];
                string fileExtensionWithVersion = fileNameWithExtension[fileNameWithExtension.IndexOf('.')..];

                string fileName = fileNameWithExtension[..fileNameWithExtension.IndexOf('.')];
                string fileExtension = fileExtensionWithVersion[1..].Contains('.') ?
                fileExtensionWithVersion[..fileExtensionWithVersion.LastIndexOf('.')] :
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

        public void MoveFileToWorkflowsDirectory(string filePath, User user)
        {
            string userWorkflowsDirectory = GetUserWorkflowsDirectory(user);

            string fileName = filePath[(filePath.LastIndexOf('\\') + 1)..];
            string newFilePath = $"{userWorkflowsDirectory}/{fileName}";

            try
            {
                File.Move(filePath, newFilePath);
            }
            catch (Exception e)
            {
                throw new Exception($"Ocorreu um erro ao tentar mover o arquivo: '{filePath}' para o diretório de workflows");
            }

        }
    }
}