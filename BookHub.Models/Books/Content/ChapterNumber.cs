using System;

namespace BookHub.Models.Books.Content;

/// <summary>
/// Модель номера главы.
/// </summary>
public sealed record class ChapterNumber
{
    public int Value { get; }

    public ChapterNumber(int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, 1);

        Value = value;
    }
}