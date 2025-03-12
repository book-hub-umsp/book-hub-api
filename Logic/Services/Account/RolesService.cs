using BookHub.API.Abstractions.Logic.Services.Account;
using BookHub.API.Abstractions.Storage;
using BookHub.API.Models.Account;
using BookHub.API.Models.Identifiers;

using Microsoft.Extensions.Logging;

namespace BookHub.API.Logic.Services.Account;

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
        IReadOnlySet<Permission> updatedPermissions,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(roleId);
        ArgumentNullException.ThrowIfNull(updatedPermissions);

        _logger.LogDebug("Changing permissions for role {RoleId}", roleId.Value);

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

        _logger.LogDebug(
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

    public Task<IReadOnlyCollection<Role>> GetAllRolesAsync(CancellationToken token)
        => _booksHubUnitOfWork.UserRoles.GetAllRolesAsync(token);

    private readonly IBooksHubUnitOfWork _booksHubUnitOfWork;
    private readonly ILogger<RolesService> _logger;
}