using BookHub.Abstractions.Logic.Services.Books.Repository;
using BookHub.Abstractions.Storage;
using BookHub.Models.Books.Repository;

using Microsoft.Extensions.Logging;

namespace BookHub.Logic.Services.Books.Repository;

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

        _logger.LogDebug("Trying to add new genre '{Genre}'", bookGenre.Value);

        await _unitOfWork.BooksGenres.AddNewGenreAsync(bookGenre, token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.LogInformation("Genre '{Genre}' was added", bookGenre.Value);
    }

    public async Task<IReadOnlyCollection<BookGenre>> GetBooksGenresAsync(CancellationToken token)
    {
        _logger.LogDebug("Receiving all existed book genres");

        var genres = await _unitOfWork.BooksGenres.GetAllGenresAsync(token);

        _logger.LogDebug("Genres {Count} were received", genres.Count);

        return genres;
    }

    public async Task RemoveBookGenreAsync(BookGenre bookGenre, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookGenre);

        _logger.LogDebug("Trying to remove genre '{Genre}'", bookGenre.Value);

        await _unitOfWork.BooksGenres.RemoveGenreAsync(bookGenre, token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.LogInformation("Genre '{Genre}' was removed with books links", bookGenre.Value);
    }

    private readonly IBooksHubUnitOfWork _unitOfWork;
    private readonly ILogger<BookGenresService> _logger;
}