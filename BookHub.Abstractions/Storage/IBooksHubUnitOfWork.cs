using BookHub.Abstractions.Storage.Repositories;

namespace BookHub.Abstractions.Storage;

/// <summary>
/// Описывает единицу работы с хранилищем книг.
/// </summary>
public interface IBooksHubUnitOfWork
{
    public IBooksRepository Books { get; }

    public IUsersRepository Users { get; }

    public Task SaveChangesAsync(CancellationToken token);
}
