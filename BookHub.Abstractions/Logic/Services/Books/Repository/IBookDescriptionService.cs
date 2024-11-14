using BookHub.Models;
using BookHub.Models.Account;
using BookHub.Models.API.Pagination;
using BookHub.Models.Books.Repository;
using BookHub.Models.CRUDS.Requests;

namespace BookHub.Abstractions.Logic.Services.Books.Repository;

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

    public Task<(IReadOnlyCollection<BookPreview>, PagePagination)> GetAuthorBooksPreviewsAsync(
        Id<User> authorId,
        PagePagging getPaginedBooks,
        CancellationToken token);

    public Task<(IReadOnlyCollection<BookPreview>, PagePagination)> GetBooksPreviewsAsync(
        PagePagging getPaginedBooks,
        CancellationToken token);
}