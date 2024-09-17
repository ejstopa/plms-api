using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using FluentValidation;

namespace Application.Abstractions.FileSystem
{
    public interface IUserWorkflowFilesService
    {
        public string GetUserWorkflowsDirectory(User user);
        public List<UserFile> GetUserUserWorkflowFiles(User user, List<string>? filterExtensions = null);
        public void MoveFileToWorkflowsDirectory(string filePath,  User user);
    }
}