using BookHub.Storage.PostgreSQL.Models;

using Microsoft.EntityFrameworkCore;

namespace BookHub.Storage.PostgreSQL.Abstractions;

/// <summary>
/// Описывает контекст для репозиториев.
/// </summary>
public interface IRepositoryContext
{
    public DbSet<User> Users { get; }

    public DbSet<Role> UserRoles { get; }

    public DbSet<Book> Books { get; }

    public DbSet<Chapter> Chapters { get; }

    public DbSet<Keyword> Keywords { get; }

    public DbSet<BookGenre> Genres { get; }

    public DbSet<FavoriteLink> FavoriteLinks { get; }

    public DbSet<KeywordLink> KeywordLinks { get; }
}