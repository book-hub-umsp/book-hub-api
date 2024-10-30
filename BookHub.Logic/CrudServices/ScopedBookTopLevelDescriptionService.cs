using Abstractions.Logic.CrudServices;

using BookHub.Models.CRUDS.Requests;
using BookHub.Models.CRUDS.Responces;

using Microsoft.Extensions.DependencyInjection;

namespace BookHub.Logic.CrudServices;

/// <summary>
/// Прокси для сервиса обработки crud запросов к верхнеуровневому описанию книги.
/// </summary>
public sealed class ScopedBookTopLevelDescriptionService : IScopedBookTopLevelDescriptionService
{
    public ScopedBookTopLevelDescriptionService(
        IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
    }

    public Task<CommandExecutionResult> AddBookAsync(
        AddBookParams addBookParams,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(addBookParams);

        using var scope = _scopeFactory.CreateScope();

        return scope.ServiceProvider
            .GetRequiredService<IBookTopLevelDescriptionService>()
            .AddBookAsync(addBookParams, token);
    }

    public Task<CommandResultWithContent> GetBookAsync(
        GetBookParams getBookParams,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(getBookParams);

        using var scope = _scopeFactory.CreateScope();

        return scope.ServiceProvider
            .GetRequiredService<IBookTopLevelDescriptionService>()
            .GetBookAsync(getBookParams, token);
    }

    public Task<CommandExecutionResult> UpdateBookAsync(
        UpdateBookParamsBase updateBookParams,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(updateBookParams);

        using var scope = _scopeFactory.CreateScope();

        return scope.ServiceProvider
            .GetRequiredService<IBookTopLevelDescriptionService>()
            .UpdateBookAsync(updateBookParams, token);
    }

    private readonly IServiceScopeFactory _scopeFactory;
}