using BookHub.Models;
using BookHub.Models.Account;

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
    /// Изменяет permissions для роли.
    /// </summary>
    /// <param name="updatedRole">
    /// Роль с обновленными permissions.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="updatedRole"/> равен <see langword="null"/>.
    /// </exception>
    Task ChangeRolePermissionsAsync(Role updatedRole, CancellationToken token);

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
    Task ChangeUserRoleAsync(
        Id<User> userId, 
        Name<Role> clarifiedRoleName, 
        CancellationToken token);
}