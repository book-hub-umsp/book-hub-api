using BookHub.Models;
using BookHub.Models.Account;
using BookHub.Models.API.Pagination;
using BookHub.Models.Books.Content;
using BookHub.Models.Books.Repository;
using BookHub.Models.CRUDS.Requests;

using DomainBook = BookHub.Models.Books.Repository.Book;

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

    /// <exception cref="InvalidOperationException">
    /// Если автор книги не соответствует указанному в запросе.
    /// </exception>
    public Task UpdateBookContentAsync(
        UpdateBookParamsBase updateBookParams,
        CancellationToken token);

    public Task<IReadOnlyCollection<BookPreview>> GetAuthorBooksAsync(
        Id<User> authorId,
        PaginationBase pagination,
        CancellationToken token);

    public Task<IReadOnlyCollection<BookPreview>> GetBooksAsync(
        PaginationBase pagination,
        CancellationToken token);


    public Task<IReadOnlyCollection<BookPreview>> GetBooksByKeywordAsync(
        KeyWord keyword,
        PaginationBase pagination,
        CancellationToken token);

    public Task<long> GetBooksTotalCountAsync(CancellationToken token);
}