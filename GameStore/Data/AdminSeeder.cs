using GameStore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Data
{
    public static class AdminSeeder
    {
        public static void SeedAdmin(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
            var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>();

            // Apply migrations (creates tables if not exists)
            if (context.Database.IsSqlServer())
            {
                context.Database.Migrate();
            }
            else
            {
                context.Database.EnsureCreated();
            }

            Console.WriteLine("Checking for existing Admin...");

            // Only seed if no admin exists
            if (!context.Users.Any(u => u.Role == "Admin"))
            {
                var admin = new User
                {
                    UserId = Guid.NewGuid(),
                    Username = "DefaultAdmin",
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow,
                    Email = "admin123@gmail.com"
                };
                admin.PasswordHash = passwordHasher.HashPassword(admin, "Admin123!!");

                context.Users.Add(admin);
                context.SaveChanges();

                Console.WriteLine("Seeded initial Admin user: DefaultAdmin / Admin123!!");
            }
            else
            {
                Console.WriteLine("Admin already exists. Skipping seeding.");
            }
        }

    }
}

