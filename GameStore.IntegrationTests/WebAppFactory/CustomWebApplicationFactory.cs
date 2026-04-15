using GameStore.Data;
using GameStore.Entities;
using GameStore.IntegrationTests.AuthHandler;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data.Common;

namespace GameStore.IntegrationTests.WebAppFactory
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private DbConnection? _connection;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                //Isolate authentication to avoid hitting JWT during tests
                services.Configure<AuthenticationOptions>(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                });

                //AddScheme<TOptions, THandler>: This method registers a specific way to handle authentication
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
                //AuthenticationSchemeOptions because you used this as the base type in TestAuthHandler.
                //TestAuthHandler: This points directly to the custom class you wrote.


                // Robustly remove ALL traces of the real database providers.
                var descriptorsToRemove = services.Where(d =>
                    d.ServiceType == typeof(DbContextOptions<GameStoreContext>) ||
                    d.ServiceType == typeof(DbContextOptions) ||
                    d.ServiceType == typeof(DbConnection) ||
                    d.ServiceType.Name.Contains("IDbContextOptionsConfiguration")
                ).ToList();

                foreach (var descriptor in descriptorsToRemove)
                {
                    services.Remove(descriptor);
                }

                // Initialize the shared connection for the in-memory SQLite database
                _connection = new SqliteConnection("DataSource=:memory:");
                _connection.Open();

                services.AddDbContext<GameStoreContext>(options =>
                {
                    options.UseSqlite(_connection);
                });
            });
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = base.CreateHost(builder);

            using var scope = host.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
            db.Database.EnsureCreated();

            // Seed reference data if required for tests
            if (!db.Games.Any())
            {
                 for (int i = 1; i <= 5; i++)
                 {
                     db.Genres.Add(new Genre
                     {
                         Id = i,
                         Name = $"Genre{i}"
                     });

                     db.Games.Add(new Game
                     {
                         Id = i,
                         Name = $"Game{i}",
                         GenreId = i,
                         Description = "Description",
                         ReleaseDate = new DateTime(2016, 2, 16),
                         Price = 59.99M
                     });
                 }

                 db.SaveChanges();
            }

            return host;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            
            if (disposing)
            {
                _connection?.Close();
                _connection?.Dispose();
            }
        }
    }
}
