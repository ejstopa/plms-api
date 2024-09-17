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
                UserFile userfile = new(filePath);

                if (filterExtensions != null && filterExtensions.Contains(userfile.Extension))
                {
                    if (!userFiles.Where(file => file.Name == userfile.Name && file.Extension == userfile.Extension).Any())
                    {
                        userFiles.Add(userfile);
                    }
                }

            }

            return userFiles;
        }

        public void MoveFileToWorkflowsDirectory(string filePath, User user)
        {
            string userWorkflowsDirectory = GetUserWorkflowsDirectory(user);

            UserFile userfile = new (filePath);

            string newFilePath = $"{userWorkflowsDirectory}/{userfile.Name}{userfile.Extension}";

            try
            {
                File.Move(filePath, newFilePath);
            }
            catch
            {
                throw new Exception($"Ocorreu um erro ao tentar mover o arquivo: '{filePath}' para o diretório de workflows");
            }

        }


    }
}