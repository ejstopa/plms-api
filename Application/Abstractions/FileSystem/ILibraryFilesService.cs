using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Abstractions.FileSystem
{
    public interface ILibraryFilesService
    {
        public Task<List<LibraryDirectory>> GetLibraryDirectories();
    
    }
}