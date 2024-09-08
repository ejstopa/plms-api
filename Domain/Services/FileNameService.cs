using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class FileNameService : IFileNameService
    {
        public string? GetFileName(string filePath)
        {
            try
            {
                string fileNameWithExtension = filePath[(filePath.LastIndexOf('\\') + 1)..];
                string fileName = fileNameWithExtension[..fileNameWithExtension.IndexOf('.')];

                return fileName;
            }
            catch
            {
                return null;
            }
        }

        public string? GetFileExtension(string filePath)
        {
            try
            {
                string fileNameWithVersion = filePath[(filePath.IndexOf('.') + 1)..];

                string fileExtension = fileNameWithVersion.Contains('.') ?
                fileNameWithVersion[..fileNameWithVersion.IndexOf('.')] :
                fileNameWithVersion;

                return $".{fileExtension}";
            }
            catch
            {
                return null;
            }

        }

        public string? GetFileMimiType(string fileExtension)
        {
            return fileExtension switch
            {
                ".prt" => "application/x-creo-part",
                ".asm" => "application/x-creo-assembly",
                ".drw" => "application/x-creo-drawing",
                _ => null
            };
        }


    }
}