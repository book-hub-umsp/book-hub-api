using BookHub.Models;
using BookHub.Models.Books;
using BookHub.Models.Users;

using DomainBook = BookHub.Models.Books.Book;
using BookHub.Models.CRUDS.Requests;

namespace BookHub.Storage.PostgreSQL.Abstractions.Repositories;

/// <summary>
/// Описывает репозиторий для книги.
/// </summary>
public interface IBooksRepository
{
    public Task AddBookAsync(
        AddBookParams addBookParams,
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
}