using BookHub.Models;
using BookHub.Models.Account;
using System.Net.Mail;

namespace BookHub.Abstractions.Logic.Services.Account;

/// <summary>
/// Описывает сервис для управления ролями.
/// </summary>
public interface IRolesService
{
    /// <summary>
    /// Получает роль для пользователя по его электронной почте.
    /// </summary>
    /// <param name="mailAddress">
    /// Электронный адрес.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// <see cref="Role"/>
    /// если пользователь был найден,
    /// иначе - <see langword="null"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="mailAddress"/> равен <see langword="null"/>.
    /// </exception>
    Task<Role?> GetUserRoleAsync(MailAddress mailAddress, CancellationToken token);

    /// <summary>
    /// Добавляет новую роль в систему.
    /// </summary>
    /// <param name="role">
    /// Новая роль.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="role"/> равен <see langword="null"/>.
    /// </exception>
    Task AddRoleAsync(Role role, CancellationToken token);

    /// <summary>
    /// Изменяет клэймы для роли.
    /// </summary>
    /// <param name="updatedRole">
    /// Роль с обновленными клэймами.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="updatedRole"/> равен <see langword="null"/>.
    /// </exception>
    Task ChangeRoleClaimsAsync(Role updatedRole, CancellationToken token);

    /// <summary>
    /// Изменяет роль для пользователя.
    /// </summary>
    /// <param name="userId">
    /// Идентификатор пользователя.
    /// </param>
    /// <param name="clarifiedRole">
    /// Новая указанная роль.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если один из параметров равен равен <see langword="null"/>.
    /// </exception>
    Task ChangeUserRoleAsync(Id<User> userId, Role clarifiedRole, CancellationToken token);
}