using BookHub.API.Models.API;
using BookHub.API.Models.API.Pagination;
using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Abstractions.Logic.Services.Favorite;

/// <summary>
/// Описывает сервис для работы с избранными книгами пользователя
/// </summary>
public interface IUserFavoriteService
{
    /// <summary>
    /// Добавляет книгу в избранное для текущего пользователя (в сессии).
    /// </summary>
    /// <param name="bookId">
    /// Идентификатор книги.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="bookId"/> равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если книга с идентификатором <paramref name="bookId"/> отсутствует.
    /// </exception>
    public Task AddFavoriteLinkAsync(
        Id<Book> bookId,
        CancellationToken token);

    /// <summary>
    /// Получает пагинированный список превью книг
    /// из избранного для текущего пользователя (в сессии).
    /// </summary>
    /// <param name="pagePagging">
    /// Модель пагинации.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="pagePagging"/> равен <see langword="null"/>.
    /// </exception>
    public Task<NewsItems<BookPreview>> GetUsersFavoritesPreviewsAsync(
        PagePagging pagePagging,
        CancellationToken token);

    /// <summary>
    /// Удаляет книгу из избранного для текущего пользователя (в сессии).
    /// </summary>
    /// <param name="bookId">
    /// Идентификатор книги.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="bookId"/> равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если книга с идентификатором <paramref name="bookId"/> отсутствует.
    /// </exception>
    public Task RemoveFavoriteLinkAsync(
        Id<Book> bookId,
        CancellationToken token);
}