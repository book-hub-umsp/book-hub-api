using BookHub.Storage.PostgreSQL.Models;
using Microsoft.EntityFrameworkCore;

namespace BookHub.Storage.PostgreSQL.Abstractions;

/// <summary>
/// Описывает контекст для репозиториев.
/// </summary>
public interface IRepositoryContext
{
    public DbSet<Book> Books { get; }
}
