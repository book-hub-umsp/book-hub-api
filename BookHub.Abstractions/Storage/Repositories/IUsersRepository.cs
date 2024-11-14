﻿using System.Net.Mail;

using BookHub.Models;
using BookHub.Models.Account;
using BookHub.Models.API.Pagination;
using BookHub.Models.DomainEvents;

namespace BookHub.Abstractions.Storage.Repositories;

/// <summary>
/// Описывает хранилище пользователей.
/// </summary>
public interface IUsersRepository
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
    public Task<IReadOnlyCollection<UserProfileInfo>> GetUserProfilesInfoAsync(
        PaginationBase pagination,
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
    public Task UpdateUserAsync(UpdatedBase<User> updated, CancellationToken token);

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
    /// Если пользователя с таким идентификатом не существует в хранилище.
    /// </exception>
    public Task<UserProfileInfo> GetUserProfileInfoByIdAsync(Id<User> userId, CancellationToken token);

    /// <summary>
    /// Возвращет кол-во пользователей.
    /// </summary>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// Кол-во пользователей.
    /// </returns>
    public Task<long> GetUserCountAsync(CancellationToken token);
}