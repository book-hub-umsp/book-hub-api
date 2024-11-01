using BookHub.Models.CRUDS.Requests;
using BookHub.Models.CRUDS.Responces;

namespace Abstractions.Logic.CrudServices;

/// <summary>
/// Описывает сервис обработки crud запросов к верхнеуровневому описанию книги.
/// </summary>
public interface IBookDescriptionService
{
    public Task<CommandExecutionResult> AddBookAsync(
        AddAuthorBookParams addBookParams,
        CancellationToken token);

    public Task<CommandExecutionResult> UpdateBookAsync(
        UpdateBookParamsBase updateBookParams,
        CancellationToken token);

    public Task<CommandResultWithContent> GetBookAsync(
        GetBookParams getBookParams,
        CancellationToken token);
}