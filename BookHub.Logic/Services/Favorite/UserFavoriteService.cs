using BookHub.Abstractions.Logic.Services.Favorite;
using BookHub.Abstractions.Storage;
using BookHub.Models;
using BookHub.Models.Account;
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

    public async Task AddFavoriteLinkAsync(UserFavoriteBookLink favoriteLinkParams, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(favoriteLinkParams);

        token.ThrowIfCancellationRequested();

        _logger.LogInformation("Trying to add a favorite link with user id: {UserId} and book id: {BookId}",
            favoriteLinkParams.UserId.Value,
            favoriteLinkParams.BookId.Value);

        await _unitOfWork.FavoriteLinks.AddFavoriteLinkAsync(favoriteLinkParams, token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.LogInformation("A new favorite link with user id: {UserId} and book id: {BookId} have been successfully added",
            favoriteLinkParams.UserId.Value,
            favoriteLinkParams.UserId.Value);
    }

    public async Task<UsersFavorite> GetUsersFavoriteAsync(Id<User> userId, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userId);

        token.ThrowIfCancellationRequested();

        _logger.LogInformation("Trying to get favorite links of user with id: {UserId}",
            userId.Value);

        var usersFavorite = await _unitOfWork.FavoriteLinks.GetUsersFavoriteAsync(userId, token);

        _logger.LogInformation("User's favorite links with user id: {UserId} have been successfully received",
            userId.Value);

        return usersFavorite;
    }

    public async Task RemoveFavoriteLinkAsync(UserFavoriteBookLink favoriteLinkParams, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(favoriteLinkParams);

        token.ThrowIfCancellationRequested();

        _logger.LogInformation("Trying to remove a favorite link with user id: {UserId} and book id: {BookId}",
            favoriteLinkParams.UserId.Value,
            favoriteLinkParams.BookId.Value);

        await _unitOfWork.FavoriteLinks.RemoveFavoriteLinkAsync(favoriteLinkParams, token);

        _logger.LogInformation("A favorite link with user id: {UserId} and book id: {BookId} have been successfully removed",
            favoriteLinkParams.UserId.Value,
            favoriteLinkParams.BookId.Value);
    }
}
