using System.ComponentModel;

using BookHub.API.Abstractions.Logic.Services.Favorite;
using BookHub.API.Contracts;
using BookHub.API.Contracts.REST.Responses;
using BookHub.API.Contracts.REST.Responses.Books.Repository;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookHub.API.Service.Controllers;

/// <summary>
/// Контроллер для работы с избранным пользователя.
/// </summary>
[ApiController]
[Route("[controller]")]
[Authorize]
public sealed class FavoritesController : ControllerBase
{
    public FavoritesController(
        IUserFavoriteService favoriteService,
        ILogger<FavoritesController> logger)
    {
        _favoriteService = favoriteService
            ?? throw new ArgumentNullException(nameof(favoriteService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddFavoriteBookAsync(
        [FromQuery] long bookId,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        try
        {
            await _favoriteService.AddFavoriteLinkAsync(
                new(bookId),
                token);

            _logger.LogDebug("Request was processed with successfully result");

            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    [HttpGet]
    [ProducesResponseType<NewsItemsResponse<BookPreview>>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserFavoriteBooksAsync(
        [DefaultValue(1)][FromQuery] int pageNumber,
        [DefaultValue(5)][FromQuery] int elementsInPage,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        try
        {
            var favoriteBooksPreviews =
                await _favoriteService.GetUsersFavoritesPreviewsAsync(
                    new(pageNumber, elementsInPage),
                    token);

            _logger.LogDebug("Request was processed with successfully result");

            return Ok(
                NewsItemsResponse<BookPreview>.FromDomain(
                    favoriteBooksPreviews,
                    BookPreview.FromDomain));
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
    public async Task<IActionResult> RemoveFavoriteBookAsync(
        [FromQuery] long bookId,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        try
        {
            await _favoriteService.RemoveFavoriteLinkAsync(new(bookId), token);

            _logger.LogDebug("Request was processed with successfully result");

            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    private readonly IUserFavoriteService _favoriteService;
    private readonly ILogger<FavoritesController> _logger;
}