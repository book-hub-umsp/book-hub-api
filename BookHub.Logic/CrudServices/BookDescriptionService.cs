using Abstractions.Logic.CrudServices;

using BookHub.Contracts.CRUDS.Responces;
using BookHub.Models.CRUDS;
using BookHub.Models.CRUDS.Requests;
using BookHub.Models.CRUDS.Responces;
using BookHub.Storage.PostgreSQL.Abstractions;

using Microsoft.Extensions.Logging;

using ContractKeyWord = BookHub.Contracts.KeyWord;

namespace BookHub.Logic.CrudServices;

/// <summary>
/// Сервис обработки crud запросов к верхнеуровневому описанию книги.
/// </summary>
public sealed class BookDescriptionService : IBookDescriptionService
{
    public BookDescriptionService(
        IBooksUnitOfWork unitOfWork,
        ILogger<BookDescriptionService> logger)
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

            await _unitOfWork.SaveChangesAsync(token);

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

            var contractContent = new BookDescriptionResponse
            {
                AuthorId = content.AuthorId.Value,
                Title = content.Description.Title.Value,
                Genre = content.Description.Genre.Value,
                Annotation = content.Description.BookAnnotation.Content,
                BookStatus = content.Status,
                CreationDate = content.CreationDate,
                LastEditTime = content.LastEditDate,
                Keywords = content.Description.KeyWords
                .Select(x => new ContractKeyWord
                {
                    Content = x.Content.Value
                })
                .ToList()
            };

            return new(CommandResult.Success, contractContent);
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

            await _unitOfWork.SaveChangesAsync(token);

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