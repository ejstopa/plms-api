using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.FileSystem;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.FileSystem
{
    public class LibraryFilesService : ILibraryFilesService
    {
        private readonly IConfiguration _configuration;

        public LibraryFilesService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<List<LibraryDirectory>> GetLibraryDirectories()
        {
            string libraryBaseDir = _configuration.GetSection("BaseDirectories")["LibraryBaseDir"]!;
            List<string> directoryPaths = [.. Directory.GetDirectories(libraryBaseDir)]; 
            List<LibraryDirectory> libraryDirectories = [];

            directoryPaths.ForEach(path => libraryDirectories.Add(new LibraryDirectory{
                Name = path[(path.IndexOf('\\') + 1) ..],
                FullPath = path
            }));


            return Task.FromResult(libraryDirectories);
        }
    }
}