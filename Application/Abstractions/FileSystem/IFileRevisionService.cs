using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Abstractions.FileSystem
{
    public interface IFileRevisionService
    {
        public void CreateFileRevision(string filePathWithVersion, User user);
        public void DeleteFileRevision(string filePathWithVersion, User user);

    }
}