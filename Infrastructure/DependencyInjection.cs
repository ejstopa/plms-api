using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstractions.FileSystem;
using Application.Abstractions.Repositories;
using Infrastructure.Data.Repositories;
using Infrastructure.FileSystem;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IModelRepository, ModelRepository>();
            services.AddScoped<IUserWorkspaceFIlesService, UserWorkspaceFilesService>();
            services.AddScoped<ILibraryFilesService, LibraryFilesService>();
        }
    }
}