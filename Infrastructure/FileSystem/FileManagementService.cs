using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.FileSystem;

namespace Infrastructure.FileSystem
{
    public class FileManagementService : IFileManagementService
    {
        public bool DeleteFile(string filePath)
        {
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool MoveFile(string previousPath, string newPath)
        {
            try
            {
                File.Move(previousPath, newPath);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}