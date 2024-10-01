using Microsoft.EntityFrameworkCore;

namespace Storage.PostgreSQL;

public sealed class BooksContext : DbContext
{
    public BooksContext(DbContextOptions<BooksContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSnakeCaseNamingConvention();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // write your mapping

        base.OnModelCreating(modelBuilder);
    }
}
