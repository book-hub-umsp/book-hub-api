using BookHub.API.Abstractions.Logic.Services.Books.Repository;
using BookHub.API.Abstractions.Storage;
using BookHub.API.Models.Books.Repository;

using Microsoft.Extensions.Logging;

namespace BookHub.API.Logic.Services.Books.Repository;

/// <summary>
/// Представляет собой сервис для работы с жанрами книг.
/// </summary>
public sealed class BookGenresService : IBookGenresService
{
    public BookGenresService(
        IBooksHubUnitOfWork unitOfWork,
        ILogger<BookGenresService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task AddBookGenreAsync(BookGenre bookGenre, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookGenre);

        token.ThrowIfCancellationRequested();

        await _unitOfWork.BooksGenres.AddNewGenreAsync(bookGenre, token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.LogInformation("Genre '{@Genre}' was added", bookGenre.Value);
    }

    public async Task<IReadOnlyCollection<BookGenre>> GetBooksGenresAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var genres = await _unitOfWork.BooksGenres.GetAllGenresAsync(token);

        _logger.LogDebug("Genres {Count} were received", genres.Count);

        return genres;
    }

    public async Task RemoveBookGenreAsync(BookGenre bookGenre, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookGenre);

        token.ThrowIfCancellationRequested();

        await _unitOfWork.BooksGenres.RemoveGenreAsync(bookGenre, token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.LogInformation("Genre '{@Genre}' was removed with books links", bookGenre);
    }

    private readonly IBooksHubUnitOfWork _unitOfWork;
    private readonly ILogger<BookGenresService> _logger;
}