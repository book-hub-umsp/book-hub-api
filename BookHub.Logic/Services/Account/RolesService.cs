using BookHub.Abstractions.Logic.Services.Account;
using BookHub.Abstractions.Storage;
using BookHub.Models;
using BookHub.Models.Account;

using Microsoft.Extensions.Logging;

namespace BookHub.Logic.Services.Account;

/// <summary>
/// Представляет сервис для управления ролями.
/// </summary>
public sealed class RolesService : IRolesService
{
    public RolesService(
        IBooksHubUnitOfWork booksHubUnitOfWork,
        ILogger<RolesService> logger)
    {
        _booksHubUnitOfWork = booksHubUnitOfWork
            ?? throw new ArgumentNullException(nameof(booksHubUnitOfWork));

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<Role?> GetUserRoleAsync(Id<User> userId, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userId);

        try
        {
            return await _booksHubUnitOfWork.UserRoles.GetUserRoleAsync(userId, token);
        }
        catch (InvalidOperationException)
        {
            _logger.LogWarning("Not found user with user id {UserId}", userId.Value);

            return null;
        }
    }

    /// <inheritdoc/>
    public async Task ChangeRolePermissionsAsync(Role updatedRole, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(updatedRole);

        _logger.LogInformation("Changing permissions for role {Name}", updatedRole.Name.Value);

        await _booksHubUnitOfWork.UserRoles.ChangeRolePermissionsAsync(updatedRole, token);

        await _booksHubUnitOfWork.SaveChangesAsync(token);

        _logger.LogInformation(
            "New {PermissionsCount} permissions for role {Name} was setted", 
            updatedRole.Permissions.Count, 
            updatedRole.Name.Value);
    }

    /// <inheritdoc/>
    public async Task ChangeUserRoleAsync(
        Id<User> userId, 
        Name<Role> clarifiedRoleName, 
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(clarifiedRoleName);

        _logger.LogInformation(
            "Setting role {Role} for user {UserId}",
            clarifiedRoleName.Value,
            userId.Value);

        await _booksHubUnitOfWork.UserRoles
            .ChangeUserRoleAsync(userId, clarifiedRoleName, token);

        await _booksHubUnitOfWork.SaveChangesAsync(token);

        _logger.LogInformation("New role for user {UserId} was setted", userId.Value);
    }

    private readonly IBooksHubUnitOfWork _booksHubUnitOfWork;
    private readonly ILogger<RolesService> _logger;
}