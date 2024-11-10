
using BookHub.Abstractions.Logic.Services;
using BookHub.Abstractions.Storage;
using BookHub.Models.CRUDS.Requests.Admins;

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

        _logger.LogInformation("Trying update role for user {UserId}", updateRoleParams.UserId);

        await _unitOfWork.Users.UpdateUserRoleAsync(updateRoleParams, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Role synchronized in storage");
    }

    private readonly IBooksHubUnitOfWork _unitOfWork;
    private readonly ILogger _logger;
}