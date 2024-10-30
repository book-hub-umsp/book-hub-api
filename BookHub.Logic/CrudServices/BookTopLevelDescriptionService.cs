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
            _logger.LogInformation("Trying adding book to storage");

            await _unitOfWork.Books.AddBookAsync(addBookParams, token);

            _logger.LogInformation("New book has been succesfully added");

            return new(CommandResult.Success);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return new(CommandResult.Failed, ex.Message);
        }
    }

    public async Task<CommandResultWithContent> GetBookAsync(
        GetBookParams getBookParams,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(getBookParams);

        try
        {
            _logger.LogInformation(
                "Trying get book {BookId} content from storage", 
                getBookParams.BookId.Value);

            var content = 
                await _unitOfWork.Books.GetBookAsync(getBookParams.BookId, token);

            _logger.LogInformation(
                "Information about book {BookId} has been received", 
                getBookParams.BookId.Value);

            return new(CommandResult.Success, content);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return new(CommandResult.Failed, content: null, ex.Message);
        }
    }

    public async Task<CommandExecutionResult> UpdateBookAsync(
        UpdateBookParamsBase updateBookParams,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(updateBookParams);

        try
        {
            _logger.LogInformation(
                "Trying update book {BookId} content on storage",
                updateBookParams.BookId.Value);

            await _unitOfWork.Books.UpdateBookContentAsync(updateBookParams, token);

            _logger.LogInformation(
                "Book {BookId} content has been updated",
                updateBookParams.BookId.Value);

            return new(CommandResult.Success);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return new(CommandResult.Failed, ex.Message);
        }
    }

    private readonly IBooksUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
}