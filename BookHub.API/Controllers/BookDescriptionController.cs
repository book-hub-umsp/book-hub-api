using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using Abstractions.Logic.Converters;
using Abstractions.Logic.CrudServices;

using BookHub.Contracts.CRUDS.Responces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ContractAddAuthorBookParams = BookHub.Contracts.CRUDS.Requests.AddAuthorBookParams;
using ContractGetBookParams = BookHub.Contracts.CRUDS.Requests.GetBookParams;
using ContractKeyWord = BookHub.Contracts.KeyWord;
using ContractUpdateBookParams = BookHub.Contracts.CRUDS.Requests.UpdateBookParams;
using DomainAddAuthorBookParams = BookHub.Models.CRUDS.Requests.AddAuthorBookParams;
using DomainGetBookParams = BookHub.Models.CRUDS.Requests.GetBookParams;
using DomainUpdateBookParams = BookHub.Models.CRUDS.Requests.UpdateBookParamsBase;

namespace BookHub.API.Controllers;

/// <summary>
/// Контроллер для работы с верхнеуровневым описанием книги.
/// </summary>
[ApiController]
[Authorize]
[Route("books")]
public class BookDescriptionController : Controller
{
    public BookDescriptionController(
        IBookDescriptionService service,
        IBookParamsConverter converter,
        ILogger<BookDescriptionController> logger)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _converter = converter ?? throw new ArgumentNullException(nameof(converter));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost]
    [Route("/add")]
    public async Task<IActionResult> AddNewAuthorBookAsync(
        [Required][NotNull] ContractAddAuthorBookParams addAuthorBookParams,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogInformation(
            "Start processing new book adding request for author {AuthorId}",
            addAuthorBookParams.AuthorId);

        try
        {
            await _service.AddBookAsync(
                (DomainAddAuthorBookParams)_converter.Convert(addAuthorBookParams),
                token);

            _logger.LogInformation("Request was processed with succesfull result");

            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            _logger.LogInformation("Request was processed with failed result");

            return BadRequest(new FailureCommandResult { FailureMessage = ex.Message });
        }
    }

    [HttpGet]
    [Route("/get/{bookId}")]
    public async Task<IActionResult> GetBookContentAsync(
        [Required][NotNull] long bookId,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogInformation(
            "Start processing book {BookId} getting request", 
            bookId);

        try
        {
            var content = await _service.GetBookAsync(
                (DomainGetBookParams)_converter.Convert(
                    new ContractGetBookParams { BookId = bookId }),
                token);

            _logger.LogInformation("Request was processed with succesfull result");

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

            return Ok(new SuccesfullCommandResult { Content = contractContent });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            _logger.LogInformation("Request was processed with failed result");

            return BadRequest(new FailureCommandResult { FailureMessage = ex.Message });
        }
    }

    [HttpPut]
    [Route("/update")]
    public async Task<IActionResult> UpdateBookAsync(
        [Required][NotNull] ContractUpdateBookParams updateParams,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogInformation(
            "Start processing book {BookId} updating request",
            updateParams.BookId);

        try
        {
            await _service.UpdateBookAsync(
                (DomainUpdateBookParams)_converter.Convert(updateParams),
                token);

            _logger.LogInformation("Request was processed with succesfull result");

            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            _logger.LogInformation("Request was processed with failed result");

            return BadRequest(new FailureCommandResult { FailureMessage = ex.Message });
        }
    }

    private readonly IBookDescriptionService _service;
    private readonly IBookParamsConverter _converter;
    private readonly ILogger _logger;
}