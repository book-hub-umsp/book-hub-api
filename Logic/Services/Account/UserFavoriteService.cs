using BookHub.API.Abstractions;
using BookHub.API.Abstractions.Logic.Services.Favorite;
using BookHub.API.Abstractions.Storage;
using BookHub.API.Models.API;
using BookHub.API.Models.API.Pagination;
using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.Identifiers;

using Microsoft.Extensions.Logging;

namespace BookHub.API.Logic.Services.Account;

/// <summary>
/// Представляет собой сервис для работы с избранными книгами пользователя
/// </summary>
public sealed class UserFavoriteService : IUserFavoriteService
{
    private readonly IBooksHubUnitOfWork _unitOfWork;

    private readonly IUserIdentityFacade _userIdentityFacade;

    private readonly ILogger<UserFavoriteService> _logger;

    public UserFavoriteService(
        IBooksHubUnitOfWork unitOfWork,
        IUserIdentityFacade userIdentityFacade,
        ILogger<UserFavoriteService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _userIdentityFacade = userIdentityFacade
            ?? throw new ArgumentNullException(nameof(userIdentityFacade));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task AddFavoriteLinkAsync(
        Id<Book> bookId,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookId);

        token.ThrowIfCancellationRequested();

        var currentUserId = _userIdentityFacade.Id!;

        _logger.LogDebug(
            "Trying to add in favorites book {BookId}" +
            " via user {UserId} request",
            bookId.Value,
            currentUserId.Value);

        await _unitOfWork.FavoriteLinks.AddFavoriteLinkAsync(
            new(currentUserId, bookId),
            token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.LogDebug(
            "A new favorite link with user id {UserId}" +
            " and book id {BookId} have been successfully added",
            currentUserId.Value,
            bookId.Value);
    }

    /// <inheritdoc/>
    public async Task<NewsItems<BookPreview>> GetUsersFavoritesPreviewsAsync(
        PagePagging pagePagging,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(pagePagging);

        token.ThrowIfCancellationRequested();

        var currentUserId = _userIdentityFacade.Id!;

        var totalFavItems =
            await _unitOfWork.FavoriteLinks.GetTotalCountUserFavoritesLinkAsync(
                currentUserId,
                token);

        var userFavoriteBookIds = await _unitOfWork.FavoriteLinks
            .GetUsersFavoriteBookIdsAsync(
                currentUserId,
                pagePagging,
                token);

        var userFavorites =
            await _unitOfWork.Books.GetBooksPreviewAsync(
                userFavoriteBookIds,
                token);

        _logger.LogDebug(
            "User's favorite books previews for user {UserId}" +
            " have been successfully received",
            currentUserId.Value);

        return new(
            userFavorites,
            new PagePagination(pagePagging, totalFavItems));
    }

    /// <inheritdoc/>
    public async Task RemoveFavoriteLinkAsync(
        Id<Book> bookId,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookId);

        token.ThrowIfCancellationRequested();

        var currentUserId = _userIdentityFacade.Id!;

        _logger.LogDebug(
            "Trying to favorites in favorites book {BookId}" +
            " via user {UserId} request",
            bookId.Value,
            currentUserId.Value);

        await _unitOfWork.FavoriteLinks.RemoveFavoriteLinkAsync(
            new(currentUserId, bookId),
            token);

        await _unitOfWork.SaveChangesAsync(token);

        _logger.LogDebug(
            "A favorite link with user id {UserId}" +
            " and book id {BookId}" +
            " have been successfully removed",
            currentUserId.Value,
            bookId.Value);
    }
}
