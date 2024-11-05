using System;
using System.Text.RegularExpressions;

namespace BookHub.Models.Books;

/// <summary>
/// Жанр книги.
/// </summary>
public sealed class BookGenre : IEquatable<BookGenre>
{
    public string Value { get; }

    public BookGenre(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        // converting to snake_case to store in db in unificate format
        Value = Regex.Replace(
            value.Trim(), 
            "([a-z])([A-Z])", "$1_$2").ToLower(); ;
    }

    public bool CompareTo(string value)
        => GetLowerClearRepresentation(value) 
        == GetLowerClearRepresentation(Value);

    public bool Equals(BookGenre? other) =>
        other is not null
        && CompareTo(other.Value);

    public override bool Equals(object? obj) => Equals(obj as BookGenre);

    public override int GetHashCode() => 
        GetLowerClearRepresentation(Value).GetHashCode();

    private static string GetLowerClearRepresentation(string value)
        => value
            .Replace("_", string.Empty)
            .ToLowerInvariant();
}