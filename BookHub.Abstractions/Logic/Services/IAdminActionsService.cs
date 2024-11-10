using BookHub.Models.CRUDS.Requests.Admins;

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
    /// Если пользователь, иницирующий запрос - не является администратором.
    /// Или если администратор сам пытается сбросить свою роль.
    /// </exception>
    public Task UpdateUserRoleAsync(
        UpdateUserRoleParams updateRoleParams, 
        CancellationToken cancellationToken);
}