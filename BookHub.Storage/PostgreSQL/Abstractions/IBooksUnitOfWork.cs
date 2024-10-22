using BookHub.Storage.PostgreSQL.Abstractions.Repositories;

namespace BookHub.Storage.PostgreSQL.Abstractions;

/// <summary>
/// Описывает единицу работы с хранилищем книг.
/// </summary>
public interface IBooksUnitOfWork
{
    public IAuthorsRepository Authors { get; }

    public IBooksRepository Books { get; }

    public IKeyWordsRepository KeyWords { get; }

    public Task SaveChangesAsync(CancellationToken token);
}
