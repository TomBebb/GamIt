using GamIt.Db.Models;
using Microsoft.EntityFrameworkCore;

namespace GamIt.Db;

public class GamesDbContext : DbContext
{
    public DbSet<Game> Games { get; set; }
    public DbSet<Genre> Genres { get; set; }
}