using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Abstractions.Storage.Repositories;

/// <summary>
/// Описывает интерфейс для работы с жанрами книг.
/// </summary>
public interface IBooksGenresRepository
{
    public Task AddNewGenreAsync(BookGenre bookGenre, CancellationToken token);

    public Task<IReadOnlyCollection<BookGenre>> GetAllGenresAsync(CancellationToken token);

    public Task RemoveGenreAsync(Id<BookGenre> genreId, CancellationToken cancellationToken);
}