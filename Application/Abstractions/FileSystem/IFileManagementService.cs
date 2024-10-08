using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Abstractions.FileSystem
{
    public interface IFileManagementService
    {
        public bool MoveFile(string previousPath, string newPath);
        public bool DeleteFile(string filePath);
    }
}