using System.Net.Mail;

using BookHub.API.Models.Account;
using BookHub.API.Models.API;
using BookHub.API.Models.DomainEvents.Account;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Abstractions.Storage.Repositories;

/// <summary>
/// Описывает хранилище пользователей.
/// </summary>
public interface IUsersRepository
{
    /// <summary>
    /// Возвращает профили пользователей.
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
    public Task<IReadOnlyCollection<UserProfileInfo>> GetUserProfilesInfoAsync(
        DataManipulation manipulation,
        CancellationToken token);

    /// <summary>
    /// Добавляет пользователя в хранилище.
    /// </summary>
    /// <param name="user">
    /// Новый регистрируемый пользователь.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// <see cref="Task"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="user"/> был <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если пользователь с таким email уже существует в системе.
    /// </exception>
    public Task AddUserAsync(RegisteringUser user, CancellationToken token);

    /// <summary>
    /// Обновляет информацию о пользователе.
    /// </summary>
    /// <param name="updated">
    /// Событие, несущее информацию об обновлении.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// <see cref="Task"/>.
    /// </returns>
    public Task UpdateUserAsync(UserUpdatedBase updated, CancellationToken token);

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
    /// Возвращает информацию о профиле пользователя по его идентификатору.
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
    /// Если <paramref name="userId"/> был <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если пользователя с таким идентификатором не существует в хранилище.
    /// </exception>
    public Task<UserProfileInfo> GetUserProfileInfoByIdAsync(Id<User> userId, CancellationToken token);

    /// <summary>
    /// Возвращает кол-во пользователей.
    /// </summary>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// Кол-во пользователей.
    /// </returns>
    public Task<long> GetUsersCountAsync(CancellationToken token);
}