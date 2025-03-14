using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Abstractions.Logic.Services.Books.Repository;

/// <summary>
/// Описывает сервис для работы с жанрами книг.
/// </summary>
public interface IBookGenresService
{
    public Task AddBookGenreAsync(BookGenre bookGenre, CancellationToken token);

    public Task<IReadOnlyCollection<BookGenre>> GetBooksGenresAsync(CancellationToken token);

    public Task RemoveBookGenreAsync(Id<BookGenre> genreId, CancellationToken token);
}