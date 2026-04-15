using GameStore.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GameStore.AuthExtensions
{
    public static class JwtAuthExtensions
    {
        public static IServiceCollection AddJwtConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            //binding JwtSettings class with appsettings.json/Jwt
            services.Configure<JwtSettings>(
                configuration.GetSection("Jwt"));

            return services;
        }


        /*---------Authentication---------*/
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            //var jwtSettings = configuration.GetSection("Jwt")
            //    .Get<JwtSettings>()
            //    ?? throw new Exception("JWT settings are missing");

            //var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                          Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }


        /*-----Authorization Roles+Policies-------*/
        public static IServiceCollection AddJwtAuthorization(
            this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // Create access
                options.AddPolicy("CanCreate", policy =>
                    policy.RequireClaim("Permission", "Create"));

                // Update access
                options.AddPolicy("CanUpdate", policy =>
                    policy.RequireClaim("Permission", "Update"));

                // Delete access (Admin only)
                options.AddPolicy("CanDelete", policy =>
                    policy.RequireRole("Admin")
                          .RequireClaim("Permission", "Delete"));
            });

            return services;
        }


    }
}
