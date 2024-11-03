using BookHub.Models.Books;
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

    public Task<IReadOnlyCollection<BookPreview>> GetAllBooksPreviews(
        CancellationToken token);

    public Task<(IReadOnlyCollection<BookPreview>, Pagination)> GetPaginedBooksPreviews(
        GetPaginedBooks getPaginedBooks,
        CancellationToken token);
}