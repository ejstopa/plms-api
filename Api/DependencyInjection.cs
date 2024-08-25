
namespace Api
{
    public static class DependencyInjection
    {
        public static void AddApiServices(this IServiceCollection services)
        {
            ConfigureCors(services);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
        
        private static void ConfigureCors(IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
                });
            });
        }
    }
}