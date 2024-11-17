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
    /// Получает роль для пользователя по его идентификатору.
    /// </summary>
    /// <param name="userId">
    /// Идентификатор пользователя.
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
    /// Если <paramref name="userId"/> равен <see langword="null"/>.
    /// </exception>
    Task<Role?> GetUserRoleAsync(Id<User> userId, CancellationToken token);

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
    /// <param name="clarifiedRoleName">
    /// Имя указанной роли.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если один из параметров равен равен <see langword="null"/>.
    /// </exception>
    Task ChangeUserRoleAsync(Id<User> userId, Name<Role> clarifiedRoleName, CancellationToken token);
}