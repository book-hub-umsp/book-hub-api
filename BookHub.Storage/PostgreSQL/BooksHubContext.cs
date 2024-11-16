using BookHub.Storage.PostgreSQL.Models;

using Microsoft.EntityFrameworkCore;

namespace BookHub.Storage.PostgreSQL;

public sealed class BooksHubContext : DbContext
{
    public DbSet<Book> Books { get; set; } = null!;

    public DbSet<BookGenre> Genres { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Role> Roles { get; set; } = null!;

    public DbSet<FavoriteLink> FavoriteLinks { get; } = null!;

    public BooksHubContext(DbContextOptions<BooksHubContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSnakeCaseNamingConvention();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        CreateUser(modelBuilder);
        CreateRole(modelBuilder);

        CreateBook(modelBuilder);
        CreateBookGenre(modelBuilder);
        CreateFavoriteLink(modelBuilder);

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
           .HasOne(x => x.Role)
           .WithMany()
           .HasForeignKey(x => x.RoleId);

        _ = modelBuilder.Entity<User>()
            .ToTable("users");
    }

    private static void CreateRole(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Role>()
            .HasKey(x => x.Id);

        _ = modelBuilder.Entity<Role>()
            .Property(x => x.Id)
            .UseIdentityAlwaysColumn();

        _ = modelBuilder.Entity<Role>()
            .Property(x => x.Claims)
            .HasColumnName("claims");

        _ = modelBuilder.Entity<Role>()
            .ToTable("roles");
    }

    private static void CreateBook(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Book>()
            .HasKey(x => x.Id);

        _ = modelBuilder.Entity<Book>()
            .Property(x => x.Id)
            .UseIdentityAlwaysColumn();

        _ = modelBuilder.Entity<Book>()
            .Property(x => x.BookAnnotation)
            .HasColumnName("annotation");

        _ = modelBuilder.Entity<Book>()
            .Property(x => x.BookStatus)
            .HasColumnName("status");

        _ = modelBuilder.Entity<Book>()
            .Property(x => x.BookGenreId)
            .HasColumnName("genre_id");

        _ = modelBuilder.Entity<Book>()
           .HasOne(x => x.Author)
           .WithMany()
           .HasForeignKey(x => x.AuthorId);

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
            .HasForeignKey(x => x.Id);

        _ = modelBuilder.Entity<BookGenre>()
            .ToTable("genres");
    }

    private static void CreateFavoriteLink(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<FavoriteLink>()
            .HasKey(l => new { l.UserId, l.BookId });

        _ = modelBuilder.Entity<FavoriteLink>()
            .HasOne(l => l.User)
            .WithMany(l => l.FavouriteBooksLinks)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<FavoriteLink>()
            .HasOne(l => l.Book)
            .WithMany(l => l.UsersFavouritesLinks)
            .HasForeignKey(l => l.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<FavoriteLink>()
            .ToTable("favourites");
    }
}