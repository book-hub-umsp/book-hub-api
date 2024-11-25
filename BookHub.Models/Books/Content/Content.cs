using System;

namespace BookHub.Models.Books.Content;

/// <summary>
/// Модель контента.
/// </summary>
public sealed record class Content
{
    public string Value { get; }

    public Content(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        Value = value;
    }
}