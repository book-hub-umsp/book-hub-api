using BookHub.Storage.PostgreSQL.Models;
using Microsoft.EntityFrameworkCore;

namespace BookHub.Storage.PostgreSQL;

public sealed class BooksContext : DbContext
{
    public DbSet<Author> Authors { get; }

    public DbSet<Book> Books { get; }

    public DbSet<KeyWord> KeyWords { get; }

    public DbSet<FavouriteLink> FavoriteLinks { get; }

    public DbSet<KeyWordLink> KeyWordsLinks { get; }

    public BooksContext(DbContextOptions<BooksContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSnakeCaseNamingConvention();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        CreateAuthor(modelBuilder);
        CreateBook(modelBuilder);
        CreateKeyWord(modelBuilder);
        CreateFavouriteLink(modelBuilder);
        CreateKeyWordLink(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private static void CreateAuthor(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Author>()
            .HasKey(x => x.Id);

        _ = modelBuilder.Entity<Author>()
            .Property(x => x.Id)
            .UseIdentityAlwaysColumn();

        _ = modelBuilder.Entity<Author>()
            .HasMany(x => x.WrittenBooks)
            .WithOne()
            .HasForeignKey(x => x.Id);
    }

    private static void CreateBook(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Book>()
            .HasKey(x => x.Id);

        _ = modelBuilder.Entity<Book>()
            .Property(x => x.Id)
            .UseIdentityAlwaysColumn();
    }

    private static void CreateKeyWord(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<KeyWord>()
            .HasKey(x => x.Id);

        _ = modelBuilder.Entity<KeyWord>()
            .Property(x => x.Id)
            .UseIdentityAlwaysColumn();
    }

    private static void CreateFavouriteLink(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<FavouriteLink>()
            .HasKey(l => new { l.AuthorId, l.BookId });

        _ = modelBuilder.Entity<FavouriteLink>()
            .HasOne(l => l.Author)
            .WithMany(l => l.FavouriteBooksLinks)
            .HasForeignKey(l => l.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<FavouriteLink>()
            .HasOne(l => l.Book)
            .WithMany(l => l.LikedUsersLinks)
            .HasForeignKey(l => l.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<FavouriteLink>()
            .ToTable("favourites");
    }

    private static void CreateKeyWordLink(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<KeyWordLink>()
            .HasKey(l => new { l.KeyWordId, l.BookId });

        _ = modelBuilder.Entity<KeyWordLink>()
            .HasOne(l => l.KeyWord)
            .WithMany(l => l.BooksLinks)
            .HasForeignKey(l => l.KeyWordId)
            .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<KeyWordLink>()
            .HasOne(l => l.Book)
            .WithMany(l => l.KeywordsLinks)
            .HasForeignKey(l => l.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<KeyWordLink>()
            .ToTable("keywords_links");
    }
}
