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
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="userId"/> равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если пользователя с идентификатором <paramref name="userId"/> не существует.
    /// </exception>
    Task<Role> GetUserRoleAsync(Id<User> userId, CancellationToken token);

    /// <summary>
    /// Изменяет permissions для роли.
    /// </summary>
    /// <param name="roleId">
    /// Идентификатор роли.
    /// </param>
    /// <param name="updatedPermissions">
    /// Обновленные permissions.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="roleId"/> или <paramref name="updatedPermissions"/>
    /// равны <see langword="null"/>.
    /// </exception>
    Task ChangeRolePermissionsAsync(
        Id<Role> roleId, 
        IReadOnlySet<Permission> updatedPermissions, 
        CancellationToken token);

    /// <summary>
    /// Изменяет роль для пользователя.
    /// </summary>
    /// <param name="userId">
    /// Идентификатор пользователя.
    /// </param>
    /// <param name="clarifiedRoleId">
    /// Идентификатор указанной роли.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Если один из параметров равен равен <see langword="null"/>.
    /// </exception>
    Task ChangeUserRoleAsync(
        Id<User> userId, 
        Id<Role> clarifiedRoleId, 
        CancellationToken token);

    /// <summary>
    /// Получает список всех ролей и их идентификаторов.
    /// </summary>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    public Task<IReadOnlyCollection<Role>> GetAllRolesAsync(CancellationToken token);
}