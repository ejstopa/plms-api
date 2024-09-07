using Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public static class DependencyInjection
    {
        public static void AddDomainServices(this IServiceCollection services){
            services.AddScoped<IModelRevisionService, ModelRevisionService>();
            services.AddScoped<IItemNameReservationService, ItemNameReservationService>();
            
        }
    }
}