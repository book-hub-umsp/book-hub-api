using BookHub.API.Models;
using BookHub.API.Models.Account;
using BookHub.API.Models.API.Pagination;
using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.Favorite;

namespace BookHub.API.Abstractions.Storage.Repositories;

/// <summary>
/// Описывает репозиторий для избранных книг у пользователя.
/// </summary>
public interface IFavoriteLinkRepository
{
    /// <summary>
    /// Добавляет книгу в избранное пользователя.
    /// </summary>
    /// <param name="favoriteLink">
    /// Модель элемента избранного.
    /// </param>
    /// <param name="token">
    /// Токен.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="favoriteLink"/> равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если отсутствует связанный пользователь или книга.
    /// Или уже существует элемент избранного
    /// с той же книгой для текущего пользователя.
    /// </exception>
    public Task AddFavoriteLinkAsync(
        UserFavoriteBookLink favoriteLink,
        CancellationToken token);

    /// <summary>
    /// Получает список идентификаторов книг пользователя.
    /// </summary>
    /// <param name="userId">
    /// Идентификатор пользователя.
    /// </param>
    /// <param name="pagePagination">
    /// Модель пагинации.
    /// </param>
    /// <param name="token">
    /// Токен.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="userId"/> или <paramref name="pagePagination"/>
    /// равны <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если отсутствует связанный пользователь.
    /// </exception>
    public Task<IReadOnlySet<Id<Book>>> GetUsersFavoriteBookIdsAsync(
        Id<User> userId,
        PagePagging pagePagination,
        CancellationToken token);

    /// <summary>
    /// Удаляет книгу из избранного пользователя.
    /// </summary>
    /// <param name="favoriteLink">
    /// Модель элемента избранного.
    /// </param>
    /// <param name="token">
    /// Токен.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="favoriteLink"/> равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если отсутствует связанный элемент избранного.
    /// </exception>
    public Task RemoveFavoriteLinkAsync(
        UserFavoriteBookLink favoriteLink,
        CancellationToken token);

    /// <summary>
    /// Получает общее число книг из избранного указанного пользователя.
    /// </summary>
    /// <param name="userId">
    /// Идентификатор пользователя.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="userId"/> равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если пользователь с идентификатором
    /// <paramref name="userId"/> отсутствует.
    /// </exception>
    public Task<long> GetTotalCountUserFavoritesLinkAsync(
        Id<User> userId,
        CancellationToken token);
}