using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data
{
    public static class GameStoreContextExtension
    {
        public static IServiceCollection AddDataService(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<GameStoreContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("GameStore")));

            return services;
        }
    }
}
