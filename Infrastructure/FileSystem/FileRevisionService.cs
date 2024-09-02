using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.FileSystem;
using Domain.Entities;

namespace Infrastructure.FileSystem
{
    public class FileRevisionService : IFileRevisionService
    {
        private readonly IUserWorkspaceFIlesService _userWorkspaceFIlesService;

        public FileRevisionService(IUserWorkspaceFIlesService userWorkspaceFIlesService)
        {
            _userWorkspaceFIlesService = userWorkspaceFIlesService;
        }

        public void CreateFileRevision(string filePathWithVersion, User user)
        {
            string userWorkspaceDirectory = _userWorkspaceFIlesService.GetUserWorkspaceDirectory(user);
            string fileName = filePathWithVersion[filePathWithVersion.LastIndexOf('\\')..filePathWithVersion.LastIndexOf('.')];

            File.Copy(filePathWithVersion, $"{userWorkspaceDirectory}\\{fileName}");
        }

        public void DeleteFileRevision(string fileNameWithExtension, User user)
        {
            string userWorkspaceDirectory = _userWorkspaceFIlesService.GetUserWorkspaceDirectory(user);
        
            File.Delete( $"{userWorkspaceDirectory}\\{fileNameWithExtension}");
        }
    }
}