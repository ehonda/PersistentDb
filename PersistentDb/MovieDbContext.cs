using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace PersistentDb;

[PublicAPI]
public class MovieDbContext : DbContext
{
    public DbSet<Movie> Movies => Set<Movie>();
    
    public MovieDbContext(DbContextOptions<MovieDbContext> options)
        : base(options)
    {
    }
}