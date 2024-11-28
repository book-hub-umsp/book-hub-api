using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using BookHub.Abstractions.Logic.Services.Books.Content;
using BookHub.Contracts.REST.Requests.Books.Content;
using BookHub.Contracts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookHub.Contracts.REST.Responses.Books.Content;

namespace BookHub.API.Controllers;

[ApiController]
[Authorize]
[Route("chapters")]
public sealed class ChaptersController : ControllerBase
{
    public ChaptersController(
        IChaptersService chaptersService,
        ILogger<ChaptersController> logger)
    {
        _chaptersService = chaptersService
            ?? throw new ArgumentNullException(nameof(chaptersService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddChapterAsync(
        [Required][NotNull] AddChapterRequest addChapterRequest,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogDebug(
            "Start processing adding chapter request for book {BookId}",
            addChapterRequest.BookId);

        try
        {
            await _chaptersService.AddChapterAsync(
                new(new(addChapterRequest.BookId), 
                    new(addChapterRequest.Content)),
                token);

            _logger.LogDebug("Request was processed with succesfull result");

            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RemoveChapterAsync(
        [FromQuery] long chapterId,
        [FromQuery] long bookId,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogDebug(
            "Start processing removing chapter {ChapterId} request" +
            " for book {BookId}",
            chapterId,
            bookId);

        try
        {
            await _chaptersService.RemoveChapterAsync(new(chapterId), new(bookId), token);

            _logger.LogDebug("Request was processed with succesfull result");

            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    [HttpGet]
    [ProducesResponseType<GetChapterContentResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetChapterContentAsync(
        [FromQuery] long chapterId,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogDebug(
            "Start processing getting chapter {ChapterId} content request" +
            chapterId);

        try
        {
            var chapter = await _chaptersService.GetChapterAsync(new(chapterId), token);

            var contract = new GetChapterContentResponse
            {
                BookId = chapter.BookId.Value,
                ChapterNumber = chapter.ChapterNumber.Value,
                Content = chapter.Content.Value
            };

            _logger.LogDebug("Request was processed with succesfull result");

            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateChapterContentAsync(
        [Required][NotNull] UpdateChapterContentRequest updateChapterRequest,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogDebug(
            "Start processing updating chapter {ChapterId} request" +
            " for book {BookId}",
            updateChapterRequest.ChapterId,
            updateChapterRequest.BookId);

        try
        {
            await _chaptersService.UpdateChapterAsync(
                new(new(updateChapterRequest.ChapterId), 
                    new(updateChapterRequest.BookId), 
                    new(updateChapterRequest.NewContent)),
                token);

            _logger.LogDebug("Request was processed with succesfull result");

            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    private readonly IChaptersService _chaptersService;
    private readonly ILogger<ChaptersController> _logger;
}