using BookHub.Contracts.CRUDS.Responces;
using BookHub.Models.Books;
using BookHub.Models.CRUDS.Requests;

namespace Abstractions.Logic.CrudServices;

/// <summary>
/// Описывает сервис обработки crud запросов к верхнеуровневому описанию книги.
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