using BookHub.Abstractions;
using BookHub.Abstractions.Logic.Services.Books.Repository;
using BookHub.Abstractions.Storage;
using BookHub.Models;
using BookHub.Models.Account;
using BookHub.Models.API;
using BookHub.Models.API.Pagination;
using BookHub.Models.Books.Repository;
using BookHub.Models.CRUDS.Requests;

using Microsoft.Extensions.Logging;

namespace BookHub.Logic.Services.Books.Repository;

/// <summary>
/// Сервис обработки crud запросов к верхнеуровневому описанию книги.
/// </summary>
public sealed class BookDescriptionService : IBookDescriptionService
{
    public BookDescriptionService(
        IBooksHubUnitOfWork unitOfWork,
        IUserIdentityFacade userIdentityFacade,
        ILogger<BookDescriptionService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _userIdentityFacade = userIdentityFacade
            ?? throw new ArgumentNullException(nameof(userIdentityFacade));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task AddBookAsync(
        AddBookParams addBookParams,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(addBookParams);

        var currentUserId = _userIdentityFacade.Id!;

        _logger.LogDebug(
            "Trying to add new book" +
            " via user {UserId} request",
            currentUserId.Value);

        await _unitOfWork.Books.AddBookAsync(currentUserId, addBookParams, token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.LogInformation("New book has been succesfully added");

        if (addBookParams.Keywords is not null)
        {
            await _unitOfWork.Books.SynchronizeKeyWordsForBook(
                currentUserId, addBookParams.Keywords, token);
        }

        _logger.LogInformation(
            "Synchronizing key words" +
            " for last created book was performed");
    }

    public async Task<Book> GetBookAsync(
        GetBookParams getBookParams,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(getBookParams);

        _logger.LogDebug(
            "Trying get book {BookId} content from storage",
            getBookParams.BookId.Value);

        var content =
            await _unitOfWork.Books.GetBookAsync(getBookParams.BookId, token);

        _logger.LogDebug(
            "Information about book {BookId} has been received",
            getBookParams.BookId.Value);

        return content;
    }

    public async Task UpdateBookAsync(
        UpdateBookParamsBase updateBookParams,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(updateBookParams);

        var currentUserId = _userIdentityFacade.Id!;

        _logger.LogDebug(
            "Trying to update book {BookId}" +
            " via user {UserId} request",
            updateBookParams.BookId.Value,
            currentUserId.Value);

        await _unitOfWork.Books.UpdateBookContentAsync(
            currentUserId, updateBookParams, token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.LogInformation(
            "Book {BookId} content has been updated",
            updateBookParams.BookId.Value);

    }

    public async Task<NewsItems<BookPreview>> GetAuthorBooksPreviewsAsync(
        Id<User> authorId,
        PagePagging pagination,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(pagination);

        var elementsTotalCount = await _unitOfWork.Books.GetBooksTotalCountAsync(token);

        _logger.LogDebug(
            "Trying to get paginated books from total" +
            " books count {Total} with settings: page number {PageNumber}" +
            " and elements in page {ElementsInPage} for author {AuthorId}",
            elementsTotalCount,
            pagination.PageNumber,
            pagination.PageSize,
            authorId.Value);

        var pagePagination = new PagePagination(
            pagination,
            elementsTotalCount);

        var booksPreviews =
            await _unitOfWork.Books.GetAuthorBooksAsync(
                authorId,
                pagination,
                token);

        _logger.LogDebug(
            "Paginated books previews for author {AuthorId} were received",
            authorId.Value);

        return new(booksPreviews, pagePagination);
    }

    public async Task<NewsItems<BookPreview>> GetBooksPreviewsAsync(
        PagePagging pagination,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(pagination);

        var elementsTotalCount = await _unitOfWork.Books.GetBooksTotalCountAsync(token);

        _logger.LogDebug(
            "Trying to get paginated books from total" +
            " books count {Total} with settings: page number {PageNumber}" +
            " and elements in page {ElementsInPage}",
            elementsTotalCount,
            pagination.PageNumber,
            pagination.PageSize);

        var pagePagination = new PagePagination(
            pagination,
            elementsTotalCount);

        var booksPreviews =
            await _unitOfWork.Books.GetBooksAsync(
                pagination,
                token);

        _logger.LogDebug("Paginated books previews were received");

        return new(booksPreviews, pagePagination);
    }

    public async Task<NewsItems<BookPreview>> GetBooksPreviewsByKeywordAsync(
        KeyWord keyword,
        PagePagging pagination, 
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(keyword);
        ArgumentNullException.ThrowIfNull(pagination);

        var elementsTotalCount = await _unitOfWork.Books.GetBooksTotalCountAsync(token);

        _logger.LogDebug(
            "Trying to get paginated books from total" +
            " books count {Total} with settings: page number {PageNumber}" +
            " and elements in page {ElementsInPage}" +
            " containing keyword '{Keyword}'",
            elementsTotalCount,
            pagination.PageNumber,
            pagination.PageSize,
            keyword.Content.Value);

        var pagePagination = new PagePagination(
            pagination,
            elementsTotalCount);

        var booksPreviews =
            await _unitOfWork.Books.GetBooksByKeywordAsync(
                keyword,
                pagination,
                token);

        _logger.LogInformation(
            "Paginated books previews containing keyword '{Keyword}' were received",
            keyword.Content.Value);

        return new(booksPreviews, pagePagination);
    }

    private readonly IBooksHubUnitOfWork _unitOfWork;
    private readonly IUserIdentityFacade _userIdentityFacade;
    private readonly ILogger _logger;
}