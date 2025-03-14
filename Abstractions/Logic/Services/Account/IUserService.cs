using System.Net.Mail;

using BookHub.API.Models.Account;
using BookHub.API.Models.API;
using BookHub.API.Models.DomainEvents;
using BookHub.API.Models.DomainEvents.Account;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Abstractions.Logic.Services.Account;

/// <summary>
/// Описывает сервис пользователей.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Пытается найти пользователя по его почте.
    /// </summary>
    /// <param name="mailAddress">
    /// Email.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// <see cref="Task{TResult}"/>.
    /// Если пользователь не найден в хранилище, результатом выполнения задачи будет <see langword="null"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="mailAddress"/> был <see langword="null"/>.
    /// </exception>
    public Task<UserProfileInfo?> FindUserProfileInfoByEmailAsync(MailAddress mailAddress, CancellationToken token);

    /// <summary>
    /// Возвращет профили пользователей.
    /// </summary>
    /// <param name="manipulation">
    /// Применяемая манипуляция над данными.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// Коллекция профилей пользователей.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="manipulation"/> был <see langword="null"/>.
    /// </exception>
    public Task<NewsItems<UserProfileInfo>> GetUserProfilesInfoAsync(
        DataManipulation manipulation,
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
    Task<UserProfileInfo> RegisterNewUserAsync(
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
    Task UpdateUserInfoAsync(UserUpdatedBase updatedUser, CancellationToken token);
}