using System;

namespace BookHub.Models.Users;

/// <summary>
/// Имя сущности.
/// </summary>
/// <typeparam name="TEntity">
/// Конкретный тип сущности.
/// </typeparam>
public sealed record class Name<TEntity>
{
    public string Value { get; }

    public Name(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }
}
