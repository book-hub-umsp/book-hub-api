using BookHub.API.Models.Account;
using BookHub.API.Models.API;
using BookHub.API.Models.Books;
using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.DomainEvents.Books;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Abstractions.Storage.Repositories;

/// <summary>
/// Описывает репозиторий для книги.
/// </summary>
public interface IBooksRepository
{
    public Task AddBookAsync(
        Id<User> userId,
        CreatingBook creatingBook,
        CancellationToken token);

    public Task<Book> GetBookByIdAsync(
        Id<Book> id,
        CancellationToken token);

    public Task UpdateBookAsync(
        Id<User> userId,
        BookUpdatedBase bookUpdated,
        CancellationToken token);

    public Task<IReadOnlyCollection<BookPreview>> GetBooksPreviewAsync(
        IReadOnlySet<Id<Book>> bookIds,
        CancellationToken token);

    public Task<IReadOnlyCollection<BookPreview>> GetBooksPreviewAsync(
        DataManipulation dataManipulation,
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
        Id<Book> bookId,
        CancellationToken token);

    public Task<long> GetBooksTotalCountAsync(CancellationToken token);
}