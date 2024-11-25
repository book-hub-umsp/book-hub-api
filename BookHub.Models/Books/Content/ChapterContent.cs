using System;

namespace BookHub.Models.Books.Content;

/// <summary>
/// Модель контента.
/// </summary>
public sealed record class ChapterContent
{
    public string Value { get; }

    public ChapterContent(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        ArgumentOutOfRangeException.ThrowIfLessThan(value.Length, MIN_SYMBOLS_NUMBER);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Length, MAX_SYMBOLS_NUMBER);

        Value = value;
    }

    private const int MIN_SYMBOLS_NUMBER = 0;
    private const int MAX_SYMBOLS_NUMBER = 40000;
}