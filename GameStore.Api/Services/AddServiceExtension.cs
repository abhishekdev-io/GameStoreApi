using GameStore.Api.Interfaces;

namespace GameStore.Api.Services
{
    public static class AddServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IGameServices, GamesServices>();
            services.AddScoped<IGenresServices, GenreServices>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}