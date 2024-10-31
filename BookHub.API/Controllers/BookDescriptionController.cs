using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using Abstractions.Logic.Converters;
using Abstractions.Logic.CrudServices;

using BookHub.Contracts.CRUDS.Responces;
using BookHub.Models.CRUDS;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ContractAddAuthorBookParams = BookHub.Contracts.CRUDS.Requests.AddAuthorBookParams;
using ContractAddBookParams = BookHub.Contracts.CRUDS.Requests.AddBookParams;
using ContractGetBookParams = BookHub.Contracts.CRUDS.Requests.GetBookParams;
using ContractUpdateBookParams = BookHub.Contracts.CRUDS.Requests.UpdateBookParams;
using DomainAddAuthorBookParams = BookHub.Models.CRUDS.Requests.AddAuthorBookParams;
using DomainAddBookParams = BookHub.Models.CRUDS.Requests.AddBookParams;
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
    public async Task<IActionResult> AddNewBookAsync(
        [Required][NotNull] ContractAddBookParams addParams,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogInformation("Start processing new book adding request");

        var result = await _service.AddBookAsync(
            (DomainAddBookParams)_converter.Convert(addParams),
            token);

        _logger.LogInformation(
            "Request was processed with '{ResultType}' result",
            result.CommandResult);

        return result.CommandResult == CommandResult.Success
            ? Ok()
            : BadRequest(new FailureCommandResult
            {
                FailureMessage = result.FailureMessage!
            });
    }

    [HttpPost]
    [Route("/addForAuthor")]
    public async Task<IActionResult> AddNewAuthorBookAsync(
        [Required][NotNull] ContractAddAuthorBookParams addAuthorBookParams,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogInformation(
            "Start processing new book adding request for author {AuthorId}",
            addAuthorBookParams.AuthorId);

        var result = await _service.AddBookAsync(
            (DomainAddAuthorBookParams)_converter.Convert(addAuthorBookParams),
            token);

        _logger.LogInformation(
            "Request was processed with '{ResultType}' result",
            result.CommandResult);

        return result.CommandResult == CommandResult.Success
            ? Ok()
            : BadRequest(new FailureCommandResult
            {
                FailureMessage = result.FailureMessage!
            });
    }

    [HttpPost]
    [Route("/get")]
    public async Task<IActionResult> GetBookContentAsync(
        [Required][NotNull] ContractGetBookParams getParams,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogInformation(
            "Start processing book {BookId} getting request",
            getParams.BookId);

        var result = await _service.GetBookAsync(
            (DomainGetBookParams)_converter.Convert(getParams),
            token);

        _logger.LogInformation(
            "Request was processed with '{ResultType}' result",
            result.CommandResult);

        return result.CommandResult == CommandResult.Success
            ? Ok(new SuccesfullCommandResult
            {
                Content = result.Content
            })
            : BadRequest(new FailureCommandResult
            {
                FailureMessage = result.FailureMessage!
            });
    }

    [HttpPost]
    [Route("/update")]
    public async Task<IActionResult> UpdateBookAsync(
        [Required][NotNull] ContractUpdateBookParams updateParams,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogInformation(
            "Start processing book {BookId} updating request",
            updateParams.BookId);

        var result = await _service.UpdateBookAsync(
            (DomainUpdateBookParams)_converter.Convert(updateParams),
            token);

        _logger.LogInformation(
            "Request was processed with '{ResultType}' result",
            result.CommandResult);

        return result.CommandResult == CommandResult.Success
            ? Ok()
            : BadRequest(new FailureCommandResult
            {
                FailureMessage = result.FailureMessage!
            });
    }

    private readonly IBookDescriptionService _service;
    private readonly IBookParamsConverter _converter;
    private readonly ILogger _logger;
}