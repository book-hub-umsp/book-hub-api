using BookHub.API.Models.Account;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Abstractions.Storage.Repositories;

/// <summary>
/// Описывает репозиторий ролей.
/// </summary>
public interface IRolesRepository
{
    /// <summary>
    /// Меняет набор <see cref="Permission"/> для существующей роли.
    /// </summary>
    /// <param name="roleId">
    /// Идентификатор роли.
    /// </param>
    /// <param name="updatedPermissions">
    /// Обновленный набор permissions.
    /// </param>
    /// <param name="token">
    /// Токен.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Если не существует роли с таким же названием.
    /// </exception>
    public Task ChangeRolePermissionsAsync(
        Id<Role> roleId,
        IReadOnlySet<Permission> updatedPermissions,
        CancellationToken token);

    /// <summary>
    /// Получает список всех ролей.
    /// </summary>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    public Task<IReadOnlyCollection<Role>> GetAllRolesAsync(CancellationToken token);

    /// <summary>
    /// Возвращает роль для пользователя с указанным идентификатором.
    /// </summary>
    /// <param name="userId">
    /// Идентификатор пользователя.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Если пользователя с идентификатором <paramref name="userId"/> не существует.
    /// </exception>
    public Task<Role> GetUserRoleAsync(Id<User> userId, CancellationToken token);

    /// <summary>
    /// Обновляет роль у пользователя.
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
    /// <exception cref="InvalidOperationException">
    /// Если пользователя с идентификатором <paramref name="userId"/> не существует.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если не существует роли с идентификатором <paramref name="clarifiedRoleId"/>.
    /// </exception>
    public Task ChangeUserRoleAsync(
        Id<User> userId,
        Id<Role> clarifiedRoleId,
        CancellationToken token);
}