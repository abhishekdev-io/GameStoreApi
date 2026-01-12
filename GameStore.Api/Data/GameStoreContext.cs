using System;
using Microsoft.EntityFrameworkCore;
using GameStore.Api.Entities;

namespace GameStore.Api.Data;

public class GameStoreContext : DbContext
{
    public GameStoreContext(DbContextOptions<GameStoreContext> options) : base(options)
    {

    }
    public DbSet<Game> Games { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure decimal precision for Price
        modelBuilder.Entity<Game>()
            .Property(g => g.Price)
            .HasPrecision(18, 2); // avoids truncation warning

        // Configure required relationship between Game and Genre
        modelBuilder.Entity<Game>()
            .HasOne(g => g.Genre)
            .WithMany(genre => genre.Games)
            .HasForeignKey(g => g.GenreId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed Genres
        modelBuilder.Entity<Genre>().HasData(
            new Genre { Id = 1, Name = "Fighting" },
            new Genre { Id = 2, Name = "Roleplaying" },
            new Genre { Id = 3, Name = "Sports" },
            new Genre { Id = 4, Name = "Racing" },
            new Genre { Id = 5, Name = "Kids and Family" }
        );

        // Seed Games
        modelBuilder.Entity<Game>().HasData(
            new Game
            {
                Id = 1,
                Name = "Street Fighter V",
                GenreId = 1,
                Description = "Classic fighting game",
                ReleaseDate = new DateTime(2016, 2, 16),
                Price = 59.99M
            },
            new Game
            {
                Id = 2,
                Name = "Final Fantasy XV",
                GenreId = 2,
                Description = "Epic RPG adventure",
                ReleaseDate = new DateTime(2016, 11, 29),
                Price = 49.99M
            },
            new Game
            {
                Id = 3,
                Name = "FIFA 23",
                GenreId = 3,
                Description = "Soccer simulation game",
                ReleaseDate = new DateTime(2022, 9, 27),
                Price = 69.99M
            },
            new Game
            {
                Id = 4,
                Name = "GTA V",
                GenreId = 2,
                Description = "Action,Crime,Story based",
                ReleaseDate = new DateTime(2012, 6, 16),
                Price = 79.89M
            }
        );

        // Optional: configure Users table for future seeding
        modelBuilder.Entity<User>()
            .Property(u => u.UserId)
            .ValueGeneratedNever(); // Needed if you seed User with a predefined GUID
    }


}
