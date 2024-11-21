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
        ILogger<BookDescriptionService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task AddBookAsync(
        AddAuthorBookParams addBookParams,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(addBookParams);

        _logger.LogDebug("Trying adding book to storage");

        await _unitOfWork.Books.AddBookAsync(addBookParams, token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.LogInformation("New book has been succesfully added");
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

        _logger.LogDebug(
            "Trying update book {BookId} content on storage",
            updateBookParams.BookId.Value);

        await _unitOfWork.Books.UpdateBookContentAsync(updateBookParams, token);

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
            pagination.Page,
            pagination.PageSize,
            authorId.Value);

        var pagePagination = new PagePagination(
            elementsTotalCount,
            pagination.Page,
            pagination.PageSize);

        var booksPreviews =
            await _unitOfWork.Books.GetAuthorBooksAsync(
                authorId,
                pagePagination,
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
            pagination.Page,
            pagination.PageSize);

        var pagePagination = new PagePagination(
            elementsTotalCount,
            pagination.Page,
            pagination.PageSize);

        var booksPreviews =
            await _unitOfWork.Books.GetBooksAsync(
                pagePagination,
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
            pagination.Page,
            pagination.PageSize,
            keyword.Content.Value);

        var pagePagination = new PagePagination(
            elementsTotalCount,
            pagination.Page,
            pagination.PageSize);

        var booksPreviews =
            await _unitOfWork.Books.GetBooksByKeywordAsync(
                keyword,
                pagePagination,
                token);

        _logger.LogInformation(
            "Paginated books previews containing keyword '{Keyword}' were received",
            keyword.Content.Value);

        return new(booksPreviews, pagePagination);
    }

    private readonly IBooksHubUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
}