using BookHub.Models.DomainEvents.Users;
using BookHub.Models.Users;

namespace BookHub.Abstractions.Logic.Services;

/// <summary>
/// Описывает сервис для обработки действий администратора.
/// </summary>
public interface IAdminActionsService
{
    /// <summary>
    /// Выполняет обновление администратором роли для пользователя.
    /// </summary>
    /// <param name="updateRoleParams">
    /// Модель с параметрами.
    /// </param>
    /// <param name="cancellationToken">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// <see cref="Task"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Если <paramref name="updateRoleParams"/> равен <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Если в текущей сессии приложения нет информации о пользователе.
    /// Если пользователь, иницирующий запрос - не является администратором.
    /// Или если администратор пытается сбросить роль другого админа.
    /// </exception>
    public Task UpdateUserRoleAsync(
        Updated<UserRole> updateRoleParams, 
        CancellationToken cancellationToken);
}