using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.FileSystem;

namespace Infrastructure.FileSystem
{
    public class FileManagementService : IFileManagementService
    {
        public void MoveFile(string previousPath, string newPath)
        {
            try
            {
                File.Move(previousPath, newPath);
            }
            catch(Exception e)
            {
                // throw new Exception($"Ocorreu um erro ao tentar mover o arquivo{previousPath}");
                throw new Exception(e.Message);
            }
        }
    }
}