using System;
using System.Collections.Generic;
using System.Linq;

namespace BookHub.Models.Account;

/// <summary>
/// Модель роли.
/// </summary>
public sealed class Role
{
    public Id<Role> Id { get; }

    public Name<Role> Name { get; }

    public IReadOnlySet<Permission> Permissions { get; }

    public Role(
        Id<Role> id,
        Name<Role> name, 
        IReadOnlyCollection<Permission> permissions)
    {
        ArgumentNullException.ThrowIfNull(id);
        Id = id;

        ArgumentNullException.ThrowIfNull(name);
        Name = new(name.Value.Trim());

        ArgumentNullException.ThrowIfNull(permissions);
        Permissions = permissions.ToHashSet();
    }
}