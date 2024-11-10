using BookHub.Models.CRUDS.Requests.Admins;

namespace BookHub.Abstractions.Logic.Services;

/// <summary>
/// Описывает сервис для обработки действий администратора.
/// </summary>
public interface IAdminActionsService
{
    public Task UpdateUserRoleAsync(
        UpdateUserRoleParams updateRoleParams, 
        CancellationToken cancellationToken);
}