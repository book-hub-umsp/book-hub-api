using BookHub.API.Abstractions;
using BookHub.API.Abstractions.Logic.Services.Books.Repository;
using BookHub.API.Abstractions.Storage;
using BookHub.API.Models.API;
using BookHub.API.Models.API.Pagination;
using BookHub.API.Models.Books;
using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.DomainEvents.Books;
using BookHub.API.Models.Identifiers;

using Microsoft.Extensions.Logging;

namespace BookHub.API.Logic.Services.Books.Repository;

/// <summary>
/// Сервис обработки crud запросов к верхнеуровневому описанию книги.
/// </summary>
public sealed class BookService : IBookService
{
    public BookService(
        IBooksHubUnitOfWork unitOfWork,
        IUserIdentityFacade userIdentityFacade,
        ILogger<BookService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _userIdentityFacade = userIdentityFacade
            ?? throw new ArgumentNullException(nameof(userIdentityFacade));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task AddBookAsync(
        CreatingBook creatingBook,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(creatingBook);

        token.ThrowIfCancellationRequested();

        var currentUserId = _userIdentityFacade.Id!;

        _logger.LogDebug(
            "Trying to add new book" +
            " via user {UserId} request",
            currentUserId.Value);

        await _unitOfWork.Books.AddBookAsync(currentUserId, creatingBook, token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.LogInformation("New book has been successfully added");
    }

    public async Task<Book> GetBookByIdAsync(
        Id<Book> id,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(id);

        var content =
            await _unitOfWork.Books.GetBookByIdAsync(id, token);

        _logger.LogDebug(
            "Information about book {BookId} has been received",
            id.Value);

        return content;
    }

    public async Task UpdateBookAsync(
        BookUpdatedBase bookUpdated,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookUpdated);

        var currentUserId = _userIdentityFacade.Id!;

        _logger.LogDebug(
            "Trying to update book {BookId}" +
            " via user {UserId} request",
            bookUpdated.EntityId.Value,
            currentUserId.Value);

        await _unitOfWork.Books.UpdateBookAsync(
            currentUserId,
            bookUpdated,
            token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.LogInformation(
            "Book {BookId} content has been updated",
            bookUpdated.EntityId.Value);
    }

    public async Task<NewsItems<BookPreview>> GetBooksPreviewsAsync(
        DataManipulation dataManipulation,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(dataManipulation);

        token.ThrowIfCancellationRequested();

        var totalCount = await _unitOfWork.Books
            .GetBooksTotalCountAsync(token);

        var previews = await _unitOfWork.Books
            .GetBooksPreviewAsync(dataManipulation, token);

        return new NewsItems<BookPreview>(
            previews,
            new PagePagination((PagePagging)dataManipulation.Pagination, totalCount));
    }

    private readonly IBooksHubUnitOfWork _unitOfWork;
    private readonly IUserIdentityFacade _userIdentityFacade;
    private readonly ILogger _logger;
}