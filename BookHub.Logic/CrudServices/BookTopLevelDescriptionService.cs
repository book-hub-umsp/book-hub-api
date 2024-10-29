using Microsoft.Extensions.DependencyInjection;

using Abstractions.Logic.CrudServices;

using BookHub.Models.CRUDS.Responces;
using Microsoft.Extensions.Logging;
using BookHub.Storage.PostgreSQL.Abstractions;
using BookHub.Models.CRUDS.Requests;
using System.Security.Principal;
using BookHub.Models.CRUDS;

namespace BookHub.Logic.CrudServices;

/// <summary>
/// Сервис обработки crud запросов к верхнеуровневому описанию книги.
/// </summary>
public sealed class BookTopLevelDescriptionService : IBookTopLevelDescriptionService
{
    public BookTopLevelDescriptionService(
        IBooksUnitOfWork unitOfWork,
        ILogger<BookTopLevelDescriptionService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<CommandExecutionResult> AddBookAsync(
        AddBookParams addBookParams,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(addBookParams);

        try
        {
            await _unitOfWork.Books.AddBookAsync(addBookParams, token);

            return new(CommandResult.Success);
        }
        catch (Exception ex)
        {
            return new(CommandResult.Failed, ex.Message);
        }
    }

    public Task<CommandResultWithContent> GetBookAsync(
        GetBookParams getBookParams,
        CancellationToken token)
    {

    }

    public Task<CommandExecutionResult> UpdateBookAsync(
        UpdateBookParamsBase updateBookParams,
        CancellationToken token)
    {

    }

    private readonly IBooksUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
}