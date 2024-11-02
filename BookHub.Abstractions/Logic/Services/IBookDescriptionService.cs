using BookHub.Models.Books;
using BookHub.Models.CRUDS.Requests;

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
}