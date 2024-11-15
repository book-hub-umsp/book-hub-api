using BookHub.Models.API.Pagination;
using BookHub.Storage.PostgreSQL.Abstractions;

namespace BookHub.Storage.PostgreSQL.Repositories;

/// <summary>
/// База для репозиториев.
/// </summary>
public abstract class RepositoryBase
{
    public IRepositoryContext Context { get; }

    protected RepositoryBase(IRepositoryContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }
}