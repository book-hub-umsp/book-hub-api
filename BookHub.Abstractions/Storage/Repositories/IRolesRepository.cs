using BookHub.Models;
using BookHub.Models.Account;

namespace BookHub.Abstractions.Storage.Repositories;

/// <summary>
/// Описывает репозиторий ролей.
/// </summary>
public interface IRolesRepository
{
    /// <summary>
    /// Добавляет новую роль с набором <see cref="ClaimType"/>.
    /// </summary>
    /// <param name="role">
    /// Модель роли.
    /// </param>
    /// <param name="token">
    /// Токен.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Если уже существует роль с таким же названием.
    /// </exception>
    public Task AddRoleAsync(Role role, CancellationToken token);

    /// <summary>
    /// Меняет набор <see cref="ClaimType"/> для существующей роли.
    /// </summary>
    /// <param name="updatedRole">
    /// Модель роли.
    /// </param>
    /// <param name="token">
    /// Токен.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Если не существует роли с таким же названием.
    /// </exception>
    public Task ChangeRoleClaimsAsync(Role updatedRole, CancellationToken token);

    /// <summary>
    /// Получает список всех ролей и их клэймов.
    /// </summary>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    public Task<IReadOnlyCollection<Role>> GetAllRolesAsync(CancellationToken token);

    /// <summary>
    /// Возвращает роль для указанного пользователя.
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
    /// <param name="clarifiedRole">
    /// Указанная роль.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Если пользователя с идентификатором <paramref name="userId"/> не существует.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если не существует роли с таким же названием.
    /// </exception>
    public Task ChangeUserRoleAsync(
        Id<User> userId, 
        Role clarifiedRole, 
        CancellationToken token);
}