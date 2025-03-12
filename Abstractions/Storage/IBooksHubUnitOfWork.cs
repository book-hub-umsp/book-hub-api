using BookHub.API.Abstractions.Storage.Repositories;

namespace BookHub.API.Abstractions.Storage;

/// <summary>
/// Описывает единицу работы с хранилищем книг.
/// </summary>
public interface IBooksHubUnitOfWork
{
    public IBooksRepository Books { get; }

    public IBookPartitionsRepository Chapters { get; }

    public IRolesRepository UserRoles { get; }

    public IUsersRepository Users { get; }

    public IBooksGenresRepository BooksGenres { get; }

    public IFavoriteLinkRepository FavoriteLinks { get; }

    public Task SaveChangesAsync(CancellationToken token);
}
