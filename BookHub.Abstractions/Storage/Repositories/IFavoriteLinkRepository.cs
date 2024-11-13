using BookHub.Models;
using BookHub.Models.Account;
using BookHub.Models.Favorite;

namespace BookHub.Abstractions.Storage.Repositories;

/// <summary>
/// Описывает репозиторий для избранных книг у пользователя.
/// </summary>
public interface IFavoriteLinkRepository
{
    public Task AddFavoriteLinkAsync(UserFavoriteBookLink favoriteLinkParams, CancellationToken token);

    public Task<UsersFavorite> GetUsersFavoriteAsync(Id<User> userId, CancellationToken token);

    public Task RemoveFavoriteLinkAsync(UserFavoriteBookLink favoriteLinkParams, CancellationToken token);
}
