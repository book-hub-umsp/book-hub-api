using BookHub.Models;
using BookHub.Models.Account;
using BookHub.Models.API.Pagination;
using BookHub.Models.Books.Repository;
using BookHub.Models.Favorite;

namespace BookHub.Abstractions.Storage.Repositories;

/// <summary>
/// Описывает репозиторий для избранных книг у пользователя.
/// </summary>
public interface IFavoriteLinkRepository
{
    public Task AddFavoriteLinkAsync(
        UserFavoriteBookLink favoriteLink,
        CancellationToken token);

    public Task<IReadOnlyCollection<BookPreview>> GetUsersFavoriteAsync(
        Id<User> userId, 
        PagePagging pagePagination, 
        CancellationToken token);

    public Task RemoveFavoriteLinkAsync(
        UserFavoriteBookLink favoriteLink, 
        CancellationToken token);

    public Task<long> GetTotalCountFavoriteLinkAsync(
        CancellationToken token);
}
