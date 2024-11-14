using BookHub.Models;
using BookHub.Models.Account;
using BookHub.Models.API.Pagination;
using BookHub.Models.DomainEvents;

namespace BookHub.Logic.Services.Account;

/// <summary>
/// Описывает сервис пользователей.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Возвращет профили пользователей.
    /// </summary>
    /// <param name="pagination">
    /// Применяемая пагинация.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// Коллекция профилей пользователей.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="pagination"/> был <see langword="null"/>.
    /// </exception>
    public Task<PaginatedItems<PagePagination, UserProfileInfo>> GetUserProfileInfosAsync(
        PagePagging pagination,
        CancellationToken token);

    /// <summary>
    /// Получает информацию о профиле пользователя по его идентификатору.
    /// </summary>
    /// <param name="userId">
    /// Идентификатор пользователя.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// <see cref="Task{TResult}"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="userId"/> был равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если пользователя с таким идентификатором не существет.
    /// </exception>
    Task<UserProfileInfo> GetUserProfileInfoAsync(Id<User> userId, CancellationToken token);

    /// <summary>
    /// Регистрирует новый или возращает существующий профиль пользователя.
    /// </summary>
    /// <param name="registeringUser">
    /// Регистрирующийся пользователь.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// <see cref="Task"/>.
    /// </returns>
    Task<UserProfileInfo> RegisterNewUserOrGetExistingAsync(
        RegisteringUser registeringUser,
        CancellationToken token);

    /// <summary>
    /// Обновляет данные о пользователе.
    /// </summary>
    /// <param name="updatedUser">
    /// Обновление по пользователю.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// <see cref="Task"/>.
    /// </returns>
    Task UpdateUserInfoAsync(UpdatedBase<User> updatedUser, CancellationToken token);
}