using System.Threading;

using BookHub.Abstractions;
using BookHub.Abstractions.Logic.Services;
using BookHub.Abstractions.Storage;
using BookHub.Models;
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
        IHttpUserIdentityFacade userIdentityFacade,
        ILogger<AdminActionsService> logger)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _userIdentityFacade = userIdentityFacade 
            ?? throw new ArgumentNullException(nameof(userIdentityFacade));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task UpdateUserRoleAsync(
        Updated<UserRole> updateRoleParams,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(updateRoleParams);

        var currentUserId = await GetCurrentUserIdFromSessionAsync(cancellationToken);

        if (currentUserId != updateRoleParams.Id)
        {
            _logger.LogInformation(
                "Checking admin actions existense for user {ModifiedUserId}",
                updateRoleParams.Id);

            var isModifiedUserAdmin = 
                await _unitOfWork.Users.HasAdminOptions(
                    updateRoleParams.Id, 
                    cancellationToken);

            if (isModifiedUserAdmin)
            {
                if (updateRoleParams.Attribute != UserRole.Admin)
                {
                    throw new InvalidOperationException(
                        "Administrator can not reset role for other administrattor.");
                }

                _logger.LogInformation("Update role request is redundant");

                return;
            }
        }

        _logger.LogInformation(
            "Trying update role for user {UserId}", 
            updateRoleParams.Id);

        await _unitOfWork.Users.UpdateUserAsync(
            updateRoleParams,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(
            "Role synchronized in storage for user {UserId}", 
            updateRoleParams.Id);
    }

    private async Task<Id<User>> GetCurrentUserIdFromSessionAsync(CancellationToken token)
    {
        var currentUserId = _userIdentityFacade.Id
            ?? throw new InvalidOperationException("No user existense in current session.");

        _logger.LogInformation(
            "Verifying admin actions existense for user {CurrentUserId}",
            currentUserId.Value);

        if (!await _unitOfWork.Users.HasAdminOptions(currentUserId, token))
        {
            throw new InvalidOperationException(
                $"User with id {currentUserId.Value} has not admin options.");
        }

        return currentUserId;
    }

    private readonly IHttpUserIdentityFacade _userIdentityFacade;
    private readonly IBooksHubUnitOfWork _unitOfWork;
    private readonly ILogger<AdminActionsService> _logger;
}