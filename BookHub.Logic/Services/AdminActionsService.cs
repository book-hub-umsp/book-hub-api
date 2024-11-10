using BookHub.Abstractions.Logic.Services;
using BookHub.Abstractions.Storage;
using BookHub.Models.CRUDS.Requests.Admins;
using BookHub.Models.DomainEvents.Users;
using BookHub.Models.Users;

using Microsoft.Extensions.Logging;

namespace BookHub.Logic.Services;

/// <summary>
/// Сервис для обработки действий администратора.
/// </summary>
public sealed class AdminActionsService : IAdminActionsService
{
    public AdminActionsService(
        IBooksHubUnitOfWork unitOfWork,
        ILogger<AdminActionsService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task UpdateUserRoleAsync(
        UpdateUserRoleParams updateRoleParams,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updateRoleParams);

        _logger.LogInformation(
            "Verifying admin actions existense for user {AdminId}", 
            updateRoleParams.AdminId);

        if (!await _unitOfWork.Users.HasAdminOptions(updateRoleParams.AdminId, cancellationToken))
        {
            throw new InvalidOperationException(
                $"User with id {updateRoleParams.AdminId} has not admin options.");
        }

        if (updateRoleParams.AdminId == updateRoleParams.ModifiedUserId
            && updateRoleParams.NewRole != Models.Users.UserRole.Admin) 
        {
            throw new InvalidOperationException("Administrator can not reset his role.");
        }

        _logger.LogInformation(
            "Trying update role for user {UserId}", 
            updateRoleParams.ModifiedUserId);

        await _unitOfWork.Users.UpdateUserAsync(
            new Updated<UserRole>(
                updateRoleParams.ModifiedUserId, 
                updateRoleParams.NewRole), 
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Role synchronized in storage for user {UserId}", 
            updateRoleParams.ModifiedUserId);
    }

    private readonly IBooksHubUnitOfWork _unitOfWork;
    private readonly ILogger<AdminActionsService> _logger;
}