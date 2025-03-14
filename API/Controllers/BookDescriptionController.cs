using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using BookHub.API.Abstractions.Logic.Services.Books.Repository;
using BookHub.API.Contracts;
using BookHub.API.Contracts.REST.Requests.Books.Repository;
using BookHub.API.Contracts.REST.Responses.Books.Repository;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookHub.API.Service.Controllers;

/// <summary>
/// Контроллер для работы с верхнеуровневым описанием книги.
/// </summary>
[ApiController]
[Route("books")]
public sealed class BookDescriptionController : ControllerBase
{
    public BookDescriptionController(
        IBookService service,
        ILogger<BookDescriptionController> logger)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [Route("{bookId}")]
    [ProducesResponseType<BookDescriptionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<BookDescriptionResponse>> GetBookDescriptionAsync(
        [Required] long bookId,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        try
        {
            var book = await _service.GetBookByIdAsync(new(bookId), token);

            _logger.LogDebug("Request was processed with successfully result");

            return Ok(BookDescriptionResponse.FromDomain(book));
        }
        catch (Exception ex)
        when (ex is InvalidOperationException or ArgumentException)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddBookAsync(
        [Required][NotNull] AddBookRequest addBookRequest,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        try
        {
            await _service.AddBookAsync(addBookRequest.ToDomain(), token);

            _logger.LogDebug("Request was processed with successfully result");

            return Ok();
        }
        catch (Exception ex)
        when (ex is InvalidOperationException or ArgumentException)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    [HttpPatch]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateBookAsync(
        [Required][NotNull] UpdateBookRequest updateBookRequest,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogDebug(
            "Start processing book {BookId} updating request",
            updateBookRequest.BookId);

        try
        {
            await _service.UpdateBookAsync(
                updateBookRequest.ToDomain(),
                token);

            _logger.LogInformation("Request was processed with succesfull result");

            return Ok();
        }
        catch (Exception ex)
        when (ex is InvalidOperationException or ArgumentException)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    private readonly IBookService _service;
    private readonly ILogger _logger;
}