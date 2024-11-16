using BookHub.Models.Account;
using BookHub.Models.Favorite;
using BookHub.Models;

namespace BookHub.Abstractions.Logic.Services.Favorite;

/// <summary>
/// Описывает сервис для работы с избранными книгами пользователя
/// </summary>
public interface IUserFavoriteService
{
    public Task AddFavoriteLinkAsync(
        UserFavoriteBookLink favoriteLinkParams, 
        CancellationToken token);

    public Task<UsersFavorite> GetUsersFavoriteAsync(
        Id<User> userId, 
        CancellationToken token);

    public Task RemoveFavoriteLinkAsync(
        UserFavoriteBookLink favoriteLinkParams, 
        CancellationToken token);
}
