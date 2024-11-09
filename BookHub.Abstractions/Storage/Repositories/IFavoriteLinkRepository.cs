using BookHub.Models;
using BookHub.Models.CRUDS.Requests;

namespace BookHub.Abstractions.Storage.Repositories;

/// <summary>
/// Описывает репозиторий для избранных книг у пользователя
/// </summary>
public interface IFavoriteLinkRepository
{
    public Task AddFavoriteLink(FavoriteLinkParams favoriteLinkParams, CancellationToken token);

    public Task<UsersFavorite> GetUsersFavorite(UserIdParams userIdParams, CancellationToken token);

    public Task RemoveFavoriteLink(FavoriteLinkParams favoriteLinkParams, CancellationToken token);

}
