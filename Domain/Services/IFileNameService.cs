using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IFileNameService
    {
        public string? GetFileName(string filePath);
        public string? GetFileExtension(string filePath);
        public string? GetFileMimiType(string fileExtension);
        
    }
}