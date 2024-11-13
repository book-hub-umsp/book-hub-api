using BookHub.Models.Books.Repository;

namespace BookHub.Abstractions.Storage.Repositories;

/// <summary>
/// Описывает интерфейс для работы с жанрами книг.
/// </summary>
public interface IBooksGenresRepository
{
    public Task AddNewGenreAsync(BookGenre bookGenre, CancellationToken token);

    public Task<IReadOnlyCollection<BookGenre>> GetAllGenresAsync(CancellationToken token);

    public Task RemoveGenreAsync(BookGenre bookGenre, CancellationToken cancellationToken);
}