using BookHub.Storage.PostgreSQL.Models;

using Microsoft.EntityFrameworkCore;

namespace BookHub.Storage.PostgreSQL;

public sealed class BooksHubContext : DbContext
{
    public DbSet<Book> Books { get; set; } = null!;

    public DbSet<Chapter> Chapters { get; set; } = null!;

    public DbSet<BookGenre> Genres { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Keyword> Keywords { get; set; } = null!;

    public DbSet<Role> UserRoles { get; set; } = null!;

    public DbSet<FavoriteLink> FavoriteLinks { get; set; } = null!;

    public DbSet<KeywordLink> KeywordLinks { get; set; } = null!;

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
        CreateChapter(modelBuilder);
        CreateKeyword(modelBuilder);
        CreateBookGenre(modelBuilder);

        CreateFavoriteLink(modelBuilder);
        CreateKeywordLink(modelBuilder);

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
           .WithMany(x => x.Users)
           .HasForeignKey(x => x.RoleId);

        _ = modelBuilder.Entity<User>()
            .ToTable("users");

        _ = modelBuilder.Entity<User>()
            .HasMany(x => x.WrittenBooks)
            .WithOne(x => x.Author)
            .HasForeignKey(x => x.AuthorId);

        _ = modelBuilder.Entity<User>()
            .HasMany(x => x.FavoriteBooksLinks)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
    }

    private static void CreateRole(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Role>()
            .HasKey(x => x.Id);

        _ = modelBuilder.Entity<Role>()
            .Property(x => x.Id)
            .UseIdentityAlwaysColumn();

        _ = modelBuilder.Entity<Role>()
            .HasMany(x => x.Users)
            .WithOne(x => x.Role)
            .HasForeignKey(x => x.RoleId);

        _ = modelBuilder.Entity<Role>()
            .Property(x => x.Permissions)
            .HasColumnName("permissions");

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
            .Property(x => x.AuthorId)
            .HasColumnName("author_id");

        _ = modelBuilder.Entity<Book>()
           .HasOne(x => x.Author)
           .WithMany(x => x.WrittenBooks)
           .HasForeignKey(x => x.AuthorId);

        _ = modelBuilder.Entity<Book>()
            .HasOne(x => x.BookGenre)
            .WithMany(x => x.Books)
            .HasForeignKey(x => x.BookGenreId);

        _ = modelBuilder.Entity<Book>()
            .HasMany(x => x.Chapters)
            .WithOne(x => x.Book)
            .HasForeignKey(x => x.BookId);

        _ = modelBuilder.Entity<Book>()
            .HasMany(x => x.UsersFavoritesLinks)
            .WithOne(x => x.Book)
            .HasForeignKey(x => x.BookId);

        _ = modelBuilder.Entity<Book>()
            .HasMany(x => x.KeywordLinks)
            .WithOne(x => x.Book)
            .HasForeignKey(x => x.BookId);
    }

    public static void CreateChapter(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Chapter>()
            .HasKey(x => x.Id);

        _ = modelBuilder.Entity<Chapter>()
            .Property(x => x.Id)
            .UseIdentityAlwaysColumn();

        _ = modelBuilder.Entity<Chapter>()
            .HasOne(x => x.Book)
            .WithMany(x => x.Chapters)
            .HasForeignKey(x => x.BookId);

        _ = modelBuilder.Entity<Chapter>()
            .ToTable("chapters");
    }

    public static void CreateKeyword(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Keyword>()
            .HasKey(x => x.Id);

        _ = modelBuilder.Entity<Keyword>()
            .Property(x => x.Id)
            .UseIdentityAlwaysColumn();

        _ = modelBuilder.Entity<Keyword>()
            .HasMany(x => x.BooksLinks)
            .WithOne(x => x.Keyword)
            .HasForeignKey(x => x.KeywordId);

        _ = modelBuilder.Entity<Keyword>()
            .ToTable("keywords");
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
            .WithOne(x => x.BookGenre)
            .HasForeignKey(x => x.BookGenreId);

        _ = modelBuilder.Entity<BookGenre>()
            .ToTable("genres");
    }

    private static void CreateFavoriteLink(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<FavoriteLink>()
            .HasKey(l => new { l.UserId, l.BookId });

        _ = modelBuilder.Entity<FavoriteLink>()
            .HasOne(l => l.User)
            .WithMany(l => l.FavoriteBooksLinks)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<FavoriteLink>()
            .HasOne(l => l.Book)
            .WithMany(l => l.UsersFavoritesLinks)
            .HasForeignKey(l => l.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<FavoriteLink>()
            .ToTable("favorites");

        _ = modelBuilder.Entity<FavoriteLink>()
            .Property(l => l.UserId)
            .HasColumnName("user_id");

        _ = modelBuilder.Entity<FavoriteLink>()
            .Property(l => l.BookId)
            .HasColumnName("book_id");
    }

    private static void CreateKeywordLink(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<KeywordLink>()
            .HasKey(x => new { x.KeywordId, x.BookId });

        _ = modelBuilder.Entity<KeywordLink>()
            .HasOne(x => x.Keyword)
            .WithMany(x => x.BooksLinks)
            .HasForeignKey(x => x.KeywordId)
            .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<KeywordLink>()
            .HasOne(x => x.Book)
            .WithMany(x => x.KeywordLinks)
            .HasForeignKey(x => x.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<KeywordLink>()
            .Property(x => x.BookId)
            .HasColumnName("book_id");

        _ = modelBuilder.Entity<KeywordLink>()
            .Property(x => x.KeywordId)
            .HasColumnName("keyword_id");

        _ = modelBuilder.Entity<KeywordLink>()
            .ToTable("keywords_books_links");
    }
}