using System;

namespace BookHub.Models.Users;

/// <summary>
/// Электорнная почта пользователя.
/// </summary>
public sealed record class Email
{
    public string Value { get; }

    public Email(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }
}
