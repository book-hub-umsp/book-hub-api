﻿using BookHub.Abstractions.Logic.Services;
using BookHub.Abstractions.Storage;
using BookHub.Models;
using BookHub.Models.Account;
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

        _logger.LogInformation("Trying adding book to storage");

        await _unitOfWork.Books.AddBookAsync(addBookParams, token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.LogInformation("New book has been succesfully added");
    }

    public async Task<Book> GetBookAsync(
        GetBookParams getBookParams,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(getBookParams);

        _logger.LogInformation(
            "Trying get book {BookId} content from storage",
            getBookParams.BookId.Value);

        var content =
            await _unitOfWork.Books.GetBookAsync(getBookParams.BookId, token);

        _logger.LogInformation(
            "Information about book {BookId} has been received",
            getBookParams.BookId.Value);

        return content;
    }

    public async Task UpdateBookAsync(
        UpdateBookParamsBase updateBookParams,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(updateBookParams);

        _logger.LogInformation(
            "Trying update book {BookId} content on storage",
            updateBookParams.BookId.Value);

        await _unitOfWork.Books.UpdateBookContentAsync(updateBookParams, token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.LogInformation(
            "Book {BookId} content has been updated",
            updateBookParams.BookId.Value);

    }

    public async Task<(IReadOnlyCollection<BookPreview>, PagePagination)> GetAuthorBooksPreviewsAsync(
        Id<User> authorId,
        PagePagging getPaginedBooks,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(getPaginedBooks);

        var elementsTotalCount = await _unitOfWork.Books.GetBooksTotalCountAsync(token);

        _logger.LogInformation(
            "Trying to get pagined books from total" +
            " books count {Total} with settings: page number {PageNumber}" +
            " and elements in page {ElementsInPage} for author {AuthorId}",
            elementsTotalCount,
            getPaginedBooks.Page,
            getPaginedBooks.PageSize,
            authorId.Value);

        var pagination = new PagePagination(
            elementsTotalCount,
            getPaginedBooks.Page,
            getPaginedBooks.PageSize);

        var booksPreviews =
            await _unitOfWork.Books.GetAuthorBooksAsync(
                authorId,
                pagination,
                token);

        _logger.LogInformation(
            "Pagined books previews for author {AuthorId} were received",
            authorId.Value);

        return (booksPreviews, pagination);
    }

    public async Task<(IReadOnlyCollection<BookPreview>, PagePagination)> GetBooksPreviewsAsync(
        PagePagging getPaginedBooks,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(getPaginedBooks);

        var elementsTotalCount = await _unitOfWork.Books.GetBooksTotalCountAsync(token);

        _logger.LogInformation(
            "Trying to get pagined books from total" +
            " books count {Total} with settings: page number {PageNumber}" +
            " and elements in page {ElementsInPage}",
            elementsTotalCount,
            getPaginedBooks.Page,
            getPaginedBooks.PageSize);

        var pagination = new PagePagination(
            elementsTotalCount,
            getPaginedBooks.Page,
            getPaginedBooks.PageSize);

        var booksPreviews =
            await _unitOfWork.Books.GetBooksAsync(
                pagination,
                token);

        _logger.LogInformation("Pagined books previews were received");

        return (booksPreviews, pagination);
    }

    private readonly IBooksHubUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
}