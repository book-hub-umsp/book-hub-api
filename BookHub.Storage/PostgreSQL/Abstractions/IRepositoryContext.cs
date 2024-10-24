using BookHub.Storage.PostgreSQL.Models;
using Microsoft.EntityFrameworkCore;

namespace BookHub.Storage.PostgreSQL.Abstractions;

/// <summary>
/// Описывает контекст для репозиториев.
/// </summary>
public interface IRepositoryContext
{
    public DbSet<Author> Authors { get; }

    public DbSet<Book> Books { get; }

    public DbSet<KeyWord> KeyWords { get; }

    public DbSet<KeyWordLink> KeyWordsLinks { get; }

    public Task SaveChangesAsync(CancellationToken token);
}
