using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Abstractions.FileSystem
{
    public interface IFileManagementService
    {
        public void MoveFile(string previousPath, string newPath);
    }
}