
using System.Reflection;
using Application.Features.WorkflowInstances.CreateWorkflowInstance;
using Application.Mapping;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped<ICreateWorkflowInstanceValidator, CreateWorkflowInstanceValidator>();
        }
    }
}