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
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IItemNameReservationRepository, ItemNameReservationRepository>();
            services.AddScoped<IItemFamilyRepository, ItemFamilyRepository>();
            services.AddScoped<IWorkflowInstanceRepository, WorkflowInstanceRepository>();
            services.AddScoped<IWorkflowTemplateRepository, WorkflowTemplateRepository>();
            services.AddScoped<IItemAtributeValueRepository, ItemAttributeValueRepository>();
            services.AddScoped<IItemAtributeRepository, ItemAttributeRepository>();
            services.AddScoped<IWorkflowInstanceValueRepository, WorkflowInstanceValueRepository>();
            services.AddScoped<IWorkflowStepForkRepository, WorkflowStepForkRepository>();

            services.AddScoped<IUserWorkspaceFIlesService, UserWorkspaceFilesService>();
            services.AddScoped<ILibraryFilesService, LibraryFilesService>();
            services.AddScoped<IFileRevisionService, FileRevisionService>();
            services.AddScoped<IUserWorkflowFilesService, UserWorkflowFilesService>();
            services.AddScoped<IFileManagementService, FileManagementService>();

        }
    }
}