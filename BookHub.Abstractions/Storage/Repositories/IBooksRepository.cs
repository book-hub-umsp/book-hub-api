using BookHub.Models;
using BookHub.Models.Account;
using BookHub.Models.API.Pagination;
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
        Id<User> user,
        AddBookParams addBookParams,
        CancellationToken token);

    public Task<DomainBook> GetBookAsync(
        Id<DomainBook> id,
        CancellationToken token);

    /// <exception cref="InvalidOperationException">
    /// Если автор книги не соответствует указанному в запросе.
    /// </exception>
    public Task UpdateBookContentAsync(
        Id<User> userId,
        UpdateBookParamsBase updateBookParams,
        CancellationToken token);

    public Task<IReadOnlyCollection<BookPreview>> GetAuthorBooksAsync(
        Id<User> authorId,
        PaggingBase pagination,
        CancellationToken token);

    public Task<IReadOnlyCollection<BookPreview>> GetBooksAsync(
        PaggingBase pagination,
        CancellationToken token);

    public Task<IReadOnlyCollection<BookPreview>> GetBooksPreviewsAsync(
        IReadOnlySet<Id<DomainBook>> bookIds,
        CancellationToken token);

    public Task<IReadOnlyCollection<BookPreview>> GetBooksByKeywordAsync(
        KeyWord keyword,
        PaggingBase pagination,
        CancellationToken token);

    /// <summary>
    /// Узнает, является ли текущий пользователь
    /// автором указанной книги.
    /// </summary>
    /// <param name="userId">
    /// Идентификатор пользователя.
    /// </param>
    /// <param name="bookId">
    /// Идентификатор книги.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="bookId"/> или <paramref name="userId"/>
    /// равны <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если книга с идентификатором <paramref name="bookId"/> не существует.
    /// </exception>
    public Task<bool> IsUserAuthorForBook(
        Id<User> userId,
        Id<DomainBook> bookId,
        CancellationToken token);

    public Task<long> GetBooksTotalCountAsync(CancellationToken token);
}