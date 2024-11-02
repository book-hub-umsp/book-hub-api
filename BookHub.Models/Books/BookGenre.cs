using System;

namespace BookHub.Models.Books;

/// <summary>
/// Жанр книги.
/// </summary>
public sealed record class BookGenre
{
    public string Value { get; }

    public BookGenre(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value;
    }
}