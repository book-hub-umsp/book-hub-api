﻿using System.ComponentModel;

using BookHub.Models.Account;

using Microsoft.AspNetCore.Authorization;

namespace BookHub.API.Authentification;

/// <summary>
/// Требование для <see cref="Models.Account.Permission"/>.
/// </summary>
public readonly record struct PermissionRequirement : IAuthorizationRequirement
{
    public Permission Permission { get; }

    public PermissionRequirement(Permission permission)
    {
        if (!Enum.IsDefined(permission))
        {
            throw new InvalidEnumArgumentException(
                nameof(permission),
                (int)permission,
                typeof(Permission));
        }
        Permission = permission;
    }
}