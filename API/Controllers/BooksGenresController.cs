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
/// Контроллер для работы с жанрами книг.
/// </summary>
[ApiController]
[Authorize]
[Route("genres")]
public sealed class BooksGenresController : ControllerBase
{
    public BooksGenresController(
        IBookGenresService service,
        ILogger<BooksGenresController> logger)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddBookGenreAsync(
        [Required][NotNull] AddNewGenreParams addNewGenreParams,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogDebug("Start processing adding new genre request");

        try
        {
            await _service.AddBookGenreAsync(new(addNewGenreParams.NewGenre), token);

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

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType<BooksGenresListResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetBooksGenresAsync(
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogDebug("Start processing getting all books genres request");

        try
        {
            var genres = await _service.GetBooksGenresAsync(token);

            _logger.LogInformation("Request was processed with succesfull result");

            var contractContent = BooksGenresListResponse.FromDomainsList(genres);

            return Ok(contractContent);
        }
        catch (Exception ex)
        when (ex is InvalidOperationException or ArgumentException)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteBookGenreAsync(
        [Required][NotNull] RemoveGenreParams removeGenreParams,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogDebug("Start processing removing genre request");

        try
        {
            await _service.RemoveBookGenreAsync(new(removeGenreParams.Genre), token);

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

    private readonly IBookGenresService _service;
    private readonly ILogger<BooksGenresController> _logger;
}