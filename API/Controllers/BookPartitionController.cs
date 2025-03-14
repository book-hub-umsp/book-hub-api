using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using BookHub.API.Abstractions.Logic.Services.Books.Content;
using BookHub.API.Contracts;
using BookHub.API.Contracts.REST.Requests.Books.Content;
using BookHub.API.Contracts.REST.Responses.Books.Content;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookHub.API.Service.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public sealed class BookPartitionController : ControllerBase
{
    public BookPartitionController(
        IBookPartitionService chaptersService,
        ILogger<BookPartitionController> logger)
    {
        _bookPartitionsService = chaptersService
            ?? throw new ArgumentNullException(nameof(chaptersService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddPartitionAsync(
        [Required][NotNull] AddPartitionRequest addChapterRequest,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        try
        {
            await _bookPartitionsService.AddPartitionAsync(
                new(new(addChapterRequest.BookId),
                    new(addChapterRequest.Content)),
                token);

            _logger.LogDebug("Request was processed with successful result");

            return Ok();
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
    public async Task<IActionResult> RemovePartitionAsync(
        [FromQuery] long bookId,
        [FromQuery] int partitionNumber,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        try
        {
            await _bookPartitionsService.RemovePartitionAsync(
                new(bookId), 
                new(partitionNumber), 
                token);

            _logger.LogDebug("Request was processed with successful result");

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
    [ProducesResponseType<PartitionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [AllowAnonymous]
    public async Task<ActionResult<PartitionResponse>> GetPartitionAsync(
        [FromQuery] long bookId,
        [FromQuery] int partitionNumber,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        try
        {
            var partition = await _bookPartitionsService.GetPartitionAsync(
                new(bookId),
                new(partitionNumber), 
                token);

            _logger.LogDebug("Request was processed with successful result");

            return Ok(PartitionResponse.FromDomain(partition));
        }
        catch (Exception ex)
        when (ex is InvalidOperationException or ArgumentException)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateChapterContentAsync(
        [Required][NotNull] UpdatePartitionContentRequest updateChapterRequest,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        try
        {
            await _bookPartitionsService.UpdatePartitionAsync(
                new(new(
                        new(updateChapterRequest.BookId), 
                        new(updateChapterRequest.PartitionNumber)),
                    new(updateChapterRequest.NewContent)),
                token);

            _logger.LogDebug("Request was processed with successful result");

            return Ok();
        }
        catch (Exception ex)
        when (ex is InvalidOperationException or ArgumentException)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    private readonly IBookPartitionService _bookPartitionsService;
    private readonly ILogger<BookPartitionController> _logger;
}