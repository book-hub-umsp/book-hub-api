using BookHub.Models.Account;
using BookHub.Models.Favorite;
using BookHub.Models;
using BookHub.Models.API.Pagination;

namespace BookHub.Abstractions.Logic.Services.Favorite;

/// <summary>
/// Описывает сервис для работы с избранными книгами пользователя
/// </summary>
public interface IUserFavoriteService
{
    public Task AddFavoriteLinkAsync(
        UserFavoriteBookLink favoriteLink, 
        CancellationToken token);

    public Task<UsersFavorite> GetUsersFavoriteAsync(
        Id<User> userId, 
        PagePagging pagePagging,
        CancellationToken token);

    public Task RemoveFavoriteLinkAsync(
        UserFavoriteBookLink favoriteLink, 
        CancellationToken token);
}
