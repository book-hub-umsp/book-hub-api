using BookHub.Models;
using BookHub.Models.Users;

namespace BookHub.Abstractions.Storage.Repositories;

/// <summary>
/// Описывает репозиторий для избранных книг у пользователя.
/// </summary>
public interface IFavoriteLinkRepository
{
    public Task AddFavoriteLink(UserFavoriteBookLink favoriteLinkParams, CancellationToken token);

    public Task<UsersFavorite> GetUsersFavorite(Id<User> userId, CancellationToken token);

    public Task RemoveFavoriteLink(UserFavoriteBookLink favoriteLinkParams, CancellationToken token);
}
