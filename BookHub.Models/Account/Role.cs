﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace BookHub.Models.Account;

/// <summary>
/// Модель роли.
/// </summary>
public sealed class Role : IEquatable<Role>
{
    public Name<Role> Name { get; }

    public IReadOnlySet<ClaimType> Claims { get; }

    public Role(
        Name<Role> name, 
        IReadOnlyCollection<ClaimType> claims)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(claims);

        Name = new(name.Value.Trim());

        Claims = claims.Any()
            ? claims.ToHashSet()
            : [ClaimType.None];
    }

    public bool CompareTo(Name<Role> name)
        => GetLowerCertainRepresentation(name)
        == GetLowerCertainRepresentation(Name);

    public bool Equals(Role? other) =>
        other is not null
        && CompareTo(other.Name);

    public override bool Equals(object? obj) => Equals(obj as Role);

    public override int GetHashCode() =>
        GetLowerCertainRepresentation(Name).GetHashCode();

    private static string GetLowerCertainRepresentation(Name<Role> name)
        => name.Value
            .Trim()
            .Replace("_", string.Empty)
            .ToLowerInvariant();
}