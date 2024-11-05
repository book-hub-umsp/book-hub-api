using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using BookHub.Abstractions.Logic.Services;
using BookHub.Contracts.REST.Requests;
using BookHub.Contracts.REST.Responces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookHub.API.Controllers;

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
    [Route("add")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddNewBookGenreAsync(
        [Required][NotNull] AddNewGenreParams addNewGenreParams,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogInformation("Start processing adding new genre request");

        try
        {
            await _service.AddBookGenreAsync(new(addNewGenreParams.NewGenre), token);

            _logger.LogInformation("Request was processed with succesfull result");

            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            _logger.LogInformation("Request was processed with failed result");

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    [HttpGet]
    [Route("get")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<BooksGenresListResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBooksGenresAsync(
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogInformation("Start processing getting all books genres request");

        try
        {
            var genres = await _service.GetBooksGenresAsync(token);

            _logger.LogInformation("Request was processed with succesfull result");

            var contractContent = BooksGenresListResponse.FromDomainsList(genres);

            return Ok(contractContent);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            _logger.LogInformation("Request was processed with failed result");

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    [HttpDelete]
    [Route("delete")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteBookGenreAsync(
        [Required][NotNull] RemoveGenreParams removeGenreParams,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogInformation("Start processing removing genre request");

        try
        {
            await _service.RemoveBookGenreAsync(new(removeGenreParams.Genre), token);

            _logger.LogInformation("Request was processed with succesfull result");

            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            _logger.LogInformation("Request was processed with failed result");

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    private readonly IBookGenresService _service;
    private readonly ILogger<BooksGenresController> _logger;
}