using System;
using System.Collections.Generic;
using System.Linq;

namespace BookHub.Models.Account;

/// <summary>
/// Модель роли.
/// </summary>
public sealed class Role
{
    public Name<Role> Name { get; }

    public IReadOnlySet<PermissionType> Permissions { get; }

    public Role(
        Name<Role> name, 
        IReadOnlyCollection<PermissionType> permissions)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(permissions);

        Name = new(name.Value.Trim());

        Permissions = permissions.Any()
            ? permissions.ToHashSet()
            : [PermissionType.None];
    }
}