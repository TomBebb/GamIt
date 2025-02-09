using System;
using System.Globalization;
using GamIt.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace GamIt.Db;

public class GamesDbContext : DbContext
{
    private const string DateFormat = "yyyy-MM-dd";
    public DbSet<Game> Games { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Series> Series { get; set; }
    public DbSet<GameGenre> GameGenres { get; set; }
    public DbSet<GameSeries> GameSeries { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={Paths.DbPath};Cache=Shared");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>().Property(g => g.ReleaseDate)
            .HasConversion(v => v.HasValue ? v.Value.ToString(DateFormat) : null,
                v => v != null
                    ? DateOnly.ParseExact(v!, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None)
                    : null);
        modelBuilder.Entity<Game>().HasIndex(g => new { g.ProviderSource, g.ProviderId }).IsUnique();
        modelBuilder.Entity<Game>().HasIndex(g => new { g.LibrarySource, g.LibraryId }).IsUnique();
        modelBuilder.Entity<Game>().HasIndex(g => g.Name).IsUnique();
        modelBuilder.Entity<Genre>().HasIndex(g => g.Name).IsUnique();
        modelBuilder.Entity<Genre>().HasIndex(g => new { g.ProviderSource, g.ProviderId }).IsUnique();
        modelBuilder.Entity<GameGenre>().HasKey(g => new { g.GameId, g.GenreId });
        modelBuilder.Entity<GameSeries>().HasKey(g => new { g.GameId, g.SeriesId });
    }
}