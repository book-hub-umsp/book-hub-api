using BookHub.Abstractions.Logic.Services.Favorite;
using BookHub.Abstractions.Storage;
using BookHub.Models;
using BookHub.Models.Account;
using BookHub.Models.API.Pagination;
using BookHub.Models.Favorite;
using Microsoft.Extensions.Logging;

namespace BookHub.Logic.Services.Favorite;

/// <summary>
/// Представляет собой сервис для работы с избранными книгами пользователя
/// </summary>
public sealed class UserFavoriteService : IUserFavoriteService
{
    private readonly IBooksHubUnitOfWork _unitOfWork;

    private readonly ILogger<UserFavoriteService> _logger;

    public UserFavoriteService(
        IBooksHubUnitOfWork unitOfWork,
        ILogger<UserFavoriteService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task AddFavoriteLinkAsync(UserFavoriteBookLink favoriteLink, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(favoriteLink);

        token.ThrowIfCancellationRequested();

        _logger.LogDebug("Trying to add a favorite link with user id {UserId} and book id {BookId}",
            favoriteLink.UserId.Value,
            favoriteLink.BookId.Value);

        await _unitOfWork.FavoriteLinks.AddFavoriteLinkAsync(favoriteLink, token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.LogInformation(
            "A new favorite link with user id {UserId} and book id {BookId} have been successfully added",
            favoriteLink.UserId.Value,
            favoriteLink.UserId.Value);
    }

    public async Task<UsersFavorite> GetUsersFavoriteAsync(
        Id<User> userId, 
        PagePagging pagePagging,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userId);

        token.ThrowIfCancellationRequested();

        var totalFavItems = await _unitOfWork.FavoriteLinks.GetTotalCountFavoriteLinkAsync(token);

        _logger.LogDebug(
            "Trying to get paginated favorite links from total" +
            " favorite links count {Total} with settings: page number {PageNumber}" +
            " and elements in page {ElementsInPage} for user with id {UserId}",
            totalFavItems,
            pagePagging.Page,
            pagePagging.PageSize,
            userId.Value);

        var pagePagination = new PagePagination(totalFavItems, pagePagging.Page, pagePagging.PageSize);

        var usersFavorite = await _unitOfWork.FavoriteLinks.GetUsersFavoriteAsync(userId, pagePagination, token);

        _logger.LogDebug(
            "User's favorite links with user id {UserId} have been successfully received",
            userId.Value);

        return usersFavorite;
    }

    public async Task RemoveFavoriteLinkAsync(UserFavoriteBookLink favoriteLink, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(favoriteLink);

        token.ThrowIfCancellationRequested();

        _logger.LogDebug(
            "Trying to remove a favorite link with user id {UserId} and book id {BookId}",
            favoriteLink.UserId.Value,
            favoriteLink.BookId.Value);

        await _unitOfWork.FavoriteLinks.RemoveFavoriteLinkAsync(favoriteLink, token);

        _logger.LogDebug(
            "A favorite link with user id {UserId} and book id {BookId} have been successfully removed",
            favoriteLink.UserId.Value,
            favoriteLink.BookId.Value);
    }
}
