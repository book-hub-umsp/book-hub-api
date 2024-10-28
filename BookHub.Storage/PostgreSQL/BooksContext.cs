using BookHub.Storage.PostgreSQL.Models;
using Microsoft.EntityFrameworkCore;

namespace BookHub.Storage.PostgreSQL;

public sealed class BooksContext : DbContext
{
    public DbSet<Book> Books { get; } = null!;

    public BooksContext(DbContextOptions<BooksContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSnakeCaseNamingConvention();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        CreateBook(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private static void CreateBook(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Book>()
            .HasKey(x => x.Id);

        _ = modelBuilder.Entity<Book>()
            .Property(x => x.Id)
            .UseIdentityAlwaysColumn();

        _ = modelBuilder.Entity<Book>()
            .Property(x => x.KeyWordsContent)
            .HasColumnType("json")
            .HasColumnName("keywords_content");
    }
}