﻿using BookHub.Models;
using BookHub.Models.Books;
using BookHub.Models.CRUDS.Requests;
using BookHub.Models.RequestSettings;
using BookHub.Models.Users;

using DomainBook = BookHub.Models.Books.Book;

namespace BookHub.Abstractions.Storage.Repositories;

/// <summary>
/// Описывает репозиторий для книги.
/// </summary>
public interface IBooksRepository
{
    public Task AddBookAsync(
        AddAuthorBookParams addBookParams,
        CancellationToken token);

    public Task<DomainBook> GetBookAsync(
        Id<DomainBook> id,
        CancellationToken token);

    public Task<bool> IsBookRelatedForCurrentAuthorAsync(
        Id<DomainBook> bookId,
        Id<User> authorId,
        CancellationToken token);

    public Task UpdateBookContentAsync(
        UpdateBookParamsBase updateBookParams,
        CancellationToken token);

    public Task<IReadOnlyCollection<BookPreview>> GetAllBooksPreviewsAsync(
        CancellationToken token);

    public Task<IReadOnlyCollection<BookPreview>> GetBooksByPaginationAsync(
        Pagination pagination,
        CancellationToken token);
}