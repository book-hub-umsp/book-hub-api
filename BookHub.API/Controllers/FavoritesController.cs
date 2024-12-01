using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using BookHub.Abstractions.Logic.Services.Favorite;
using BookHub.Contracts;
using BookHub.Contracts.REST.Requests.Favorite;
using BookHub.Contracts.REST.Responses;
using BookHub.Contracts.REST.Responses.Books.Repository;
using BookHub.Contracts.REST.Responses.Favorite;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ContractPreview = BookHub.Contracts.REST.Responses.Books.Repository.BookPreview;

namespace BookHub.API.Controllers;

/// <summary>
/// Контроллер для работы с избранным пользователя.
/// </summary>
[ApiController]
[Authorize]
[Route("favorites")]
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
        [Required][NotNull] AddFavoriteLinkRequest addFavoriteRequest,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogInformation(
            "Trying to add book {BookId} to favorites", 
            addFavoriteRequest.BookId);

        try
        {
            await _favoriteService.AddFavoriteLinkAsync(
                new(addFavoriteRequest.BookId),
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

    [HttpGet]
    [ProducesResponseType<GetUserFavoriteResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUserFavoriteBooksAsync(
        [DefaultValue(1)][FromQuery] int pageNumber,
        [DefaultValue(5)][FromQuery] int elementsInPage,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogDebug("Start processing get paginated favorite books request");

        try
        {
            var favoriteBooksPreviews = 
                await _favoriteService.GetUsersFavoritesPreviewsAsync(
                    new(pageNumber, elementsInPage),
                    token);

            _logger.LogDebug("Request was processed with succesfull result");

            return Ok(
                NewsItemsResponse<ContractPreview>.FromDomain(
                    favoriteBooksPreviews,
                    x => new ContractPreview
                    {
                        Id = x.Id.Value,
                        AuthorId = x.AuthorId.Value,
                        Genre = x.Genre.Value,
                        Title = x.Title.Value,
                        ChaptersNumbers = x.ChaptersIdsMap
                            .Select(pair => new BookChapterIdResponse
                            {
                                ChapterId = pair.Key.Value,
                                ChapterSequenceNumber = pair.Value.Value,
                            })
                            .ToArray()
                    }));
        }
        catch (ArgumentOutOfRangeException ex)
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

        _logger.LogDebug(
            "Start processing removing from favorites request" +
            " for book {BookId}",
            bookId);

        try
        {
            await _favoriteService.RemoveFavoriteLinkAsync(new(bookId), token);

            _logger.LogDebug("Request was processed with succesfull result");

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