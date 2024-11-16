using System.Net.Mail;

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
    public async Task<Role?> GetUserRoleAsync(MailAddress mailAddress, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(mailAddress);

        try
        {
            return await _booksHubUnitOfWork.UserRoles.GetUserRoleAsync(mailAddress, token);
        }
        catch (InvalidOperationException)
        {
            _logger.LogWarning("Not found user with mail address {Email}", mailAddress.Address);

            return null;
        }
    }

    /// <inheritdoc/>
    public async Task AddRoleAsync(Role role, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(role);

        _logger.LogInformation("Adding new role {Name}", role.Name.Value);

        await _booksHubUnitOfWork.UserRoles.AddRoleAsync(role, token);

        _logger.LogInformation(
            "Role {Name} with {ClaimsCount} was added", 
            role.Name.Value,
            role.Claims.Count);
    }

    /// <inheritdoc/>
    public async Task ChangeRoleClaimsAsync(Role updatedRole, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(updatedRole);

        _logger.LogInformation("Changing claims for role {Name}", updatedRole.Name.Value);

        await _booksHubUnitOfWork.UserRoles.ChangeRoleClaimsAsync(updatedRole, token);

        _logger.LogInformation(
            "New {ClaimsCount} claims for role {Name} was setted", 
            updatedRole.Claims.Count, 
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

        _logger.LogInformation("New role for user {UserId} was setted", userId.Value);
    }

    private readonly IBooksHubUnitOfWork _booksHubUnitOfWork;
    private readonly ILogger<RolesService> _logger;
}