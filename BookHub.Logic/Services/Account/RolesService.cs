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
    public async Task<Role> GetUserRoleAsync(Id<User> userId, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userId);

        return await _booksHubUnitOfWork.UserRoles.GetUserRoleAsync(userId, token);
    }

    /// <inheritdoc/>
    public async Task ChangeRolePermissionsAsync(
        Id<Role> roleId,
        IReadOnlySet<PermissionType> updatedPermissions,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(roleId);
        ArgumentNullException.ThrowIfNull(updatedPermissions);

        _logger.LogInformation("Changing permissions for role {RoleId}", roleId.Value);

        await _booksHubUnitOfWork.UserRoles
            .ChangeRolePermissionsAsync(roleId, updatedPermissions, token);

        await _booksHubUnitOfWork.SaveChangesAsync(token);

        _logger.LogInformation(
            "New {PermissionsCount} permissions for role {RoleId} was setted", 
            updatedPermissions.Count, 
            roleId.Value);
    }

    /// <inheritdoc/>
    public async Task ChangeUserRoleAsync(
        Id<User> userId, 
        Id<Role> clarifiedRoleId, 
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(clarifiedRoleId);

        _logger.LogInformation(
            "Setting role {RoleId} for user {UserId}",
            clarifiedRoleId.Value,
            userId.Value);

        await _booksHubUnitOfWork.UserRoles
            .ChangeUserRoleAsync(userId, clarifiedRoleId, token);

        await _booksHubUnitOfWork.SaveChangesAsync(token);

        _logger.LogInformation(
            "New role {RoleId} for user {UserId} was setted", 
            clarifiedRoleId.Value,
            userId.Value);
    }

    public Task<IReadOnlyCollection<(Id<Role>, Role)>> GetAllRolesAsync(CancellationToken token)
        => _booksHubUnitOfWork.UserRoles.GetAllRolesAsync(token);

    private readonly IBooksHubUnitOfWork _booksHubUnitOfWork;
    private readonly ILogger<RolesService> _logger;
}