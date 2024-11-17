using System.ComponentModel;

using BookHub.Models.Account;

using Microsoft.AspNetCore.Authorization;

namespace BookHub.API.Roles;

/// <summary>
/// Требование для <see cref="PermissionType"/>.
/// </summary>
public record struct PermissionTypeRequirement : IAuthorizationRequirement
{
    public PermissionType Permission { get; }

    public PermissionTypeRequirement(PermissionType permission)
    {
        if (!Enum.IsDefined(permission))
        {
            throw new InvalidEnumArgumentException(
                nameof(permission),
                (int)permission,
                typeof(PermissionType));
        }
    }
}