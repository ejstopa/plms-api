using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserFile
    {
        public string Name { get; private set; } = string.Empty;
        public string Extension { get; private set; } = string.Empty;
        public int Version { get; private set; }
        public string FullPath { get; private set; } = string.Empty;
        public DateTime LastModifiedAt { get; private set; }

        public UserFile(string filePath)
        {
            string fileNameWithExtension = filePath[(filePath.LastIndexOf('\\') + 1)..];
            string fileExtensionWithVersion = fileNameWithExtension[fileNameWithExtension.IndexOf('.')..];

            string fileName = fileNameWithExtension[..fileNameWithExtension.IndexOf('.')];
            string fileExtension = fileExtensionWithVersion[1..].Contains('.') ?
            fileExtensionWithVersion[..fileExtensionWithVersion.LastIndexOf('.')] :
            fileExtensionWithVersion;

            string fileVersion = fileExtensionWithVersion[1..].Contains('.') ?
            fileExtensionWithVersion[fileExtensionWithVersion.LastIndexOf('.')..] : "";

            Name = fileName;
            Extension = fileExtension;
            Version = fileVersion != "" ? int.Parse(fileVersion) : 0;
            FullPath = fileVersion != "" ? filePath.Replace(fileVersion, "") : filePath;
            LastModifiedAt = File.GetLastWriteTime(filePath);
        }

    }
}