using BookHub.Storage.PostgreSQL.Abstractions;

namespace BookHub.Storage.PostgreSQL.Repositories;

/// <summary>
/// База для репозиториев.
/// </summary>
public abstract class RepositoriesBase
{
    public IRepositoryContext Context { get; }

    protected RepositoriesBase(IRepositoryContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }
}