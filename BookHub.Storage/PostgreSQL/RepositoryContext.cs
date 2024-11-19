using BookHub.Storage.PostgreSQL.Abstractions;
using BookHub.Storage.PostgreSQL.Models;

using Microsoft.EntityFrameworkCore;

namespace BookHub.Storage.PostgreSQL;

/// <summary>
/// Контекст для репозиториев.
/// </summary>
public sealed class RepositoryContext : IRepositoryContext
{
    public DbSet<User> Users => _context.Users;

    public DbSet<Role> UserRoles => _context.UserRoles;

    public DbSet<Book> Books => _context.Books;

    public DbSet<BookGenre> Genres => _context.Genres;

    public DbSet<FavoriteLink> FavoriteLinks => _context.FavoriteLinks;

    public DbSet<KeywordLink> KeywordLinks => _context.KeywordLinks;

    public RepositoryContext(BooksHubContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    private readonly BooksHubContext _context;
}