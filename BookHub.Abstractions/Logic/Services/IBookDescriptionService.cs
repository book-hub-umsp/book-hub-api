using BookHub.Models;
using BookHub.Models.Account;
using BookHub.Models.Books.Repository;
using BookHub.Models.CRUDS.Requests;
using BookHub.Models.RequestSettings;

namespace BookHub.Abstractions.Logic.Services;

/// <summary>
/// Описывает сервис обработки верхнеуровневого описания книги.
/// </summary>
public interface IBookDescriptionService
{
    public Task AddBookAsync(
        AddAuthorBookParams addBookParams,
        CancellationToken token);

    public Task UpdateBookAsync(
        UpdateBookParamsBase updateBookParams,
        CancellationToken token);

    public Task<Book> GetBookAsync(
        GetBookParams getBookParams,
        CancellationToken token);

    public Task<IReadOnlyCollection<BookPreview>> GetAuthorBooksPreviewsAsync(
        Id<User> authorId,
        CancellationToken token);

    public Task<(IReadOnlyCollection<BookPreview>, Pagination)> GetAuthorPaginedBooksPreviewsAsync(
        Id<User> authorId,
        GetPaginedBooks getPaginedBooks,
        CancellationToken token);

    public Task<(IReadOnlyCollection<BookPreview>, Pagination)> GetPaginedBooksPreviewsAsync(
        GetPaginedBooks getPaginedBooks,
        CancellationToken token);
}