using BookHub.Storage.PostgreSQL.Models;
using Microsoft.EntityFrameworkCore;

namespace BookHub.Storage.PostgreSQL.Abstractions;

public interface IRepositoryContext
{
    public DbSet<Author> Authors { get; }

    public DbSet<Book> Books { get; }

    public DbSet<KeyWord> KeyWords { get; }

    public DbSet<FavouriteLink> FavoriteLinks { get; }

    public DbSet<KeyWordLink> KeyWordsLinks { get; }
}
