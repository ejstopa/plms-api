
using System.Reflection;
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
        }
    }
}