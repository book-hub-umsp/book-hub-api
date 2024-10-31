using BookHub.Storage.PostgreSQL.Models;

using Microsoft.EntityFrameworkCore;

namespace BookHub.Storage.PostgreSQL;

public sealed class BooksHubContext : DbContext
{
    public DbSet<Book> Books { get; } = null!;

    public DbSet<BookGenre> Genres { get; } = null!;

    public DbSet<User> Users { get; } = null!;

    public BooksHubContext(DbContextOptions<BooksHubContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSnakeCaseNamingConvention();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        CreateUser(modelBuilder);

        CreateBook(modelBuilder);
        CreateBookGenre(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private static void CreateUser(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<User>()
            .HasKey(x => x.Id);

        _ = modelBuilder.Entity<User>()
            .Property(x => x.Id)
            .UseIdentityAlwaysColumn();

        _ = modelBuilder.Entity<User>()
            .ToTable("users");
    }

    private static void CreateBook(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Book>()
            .HasKey(x => x.Id);

        _ = modelBuilder.Entity<Book>()
            .Property(x => x.Id)
            .UseIdentityAlwaysColumn();

        _ = modelBuilder.Entity<Book>()
            .HasOne(x => x.BookGenre)
            .WithMany()
            .HasForeignKey(x => x.BookGenreId);

        _ = modelBuilder.Entity<Book>()
            .Property(x => x.KeyWordsContent)
            .HasColumnType("json")
            .HasColumnName("keywords_content");
    }

    private static void CreateBookGenre(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<BookGenre>()
            .HasKey(x => x.Id);

        _ = modelBuilder.Entity<BookGenre>()
            .Property(x => x.Id)
            .UseIdentityAlwaysColumn();

        _ = modelBuilder.Entity<BookGenre>()
            .HasMany(x => x.Books)
            .WithOne()
            .HasForeignKey(x => x.BookGenreId);

        _ = modelBuilder.Entity<BookGenre>()
            .ToTable("genres");
    }
}