using BookHub.Models.DomainEvents;
using BookHub.Models.Users;
using BookHub.Models;

namespace BookHub.Abstractions.Logic.Services;

/// <summary>
/// Описывает сервис пользователей.
/// </summary>
public interface IUserService
{
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