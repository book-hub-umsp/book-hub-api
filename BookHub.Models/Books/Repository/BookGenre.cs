using System;

namespace BookHub.Models.Books.Repository;

/// <summary>
/// Жанр книги.
/// </summary>
public sealed class BookGenre : IEquatable<BookGenre>
{
    public string Value { get; }

    public BookGenre(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value.Trim();
    }

    public bool CompareTo(string value)
        => GetLowerCertainRepresentation(value)
        == GetLowerCertainRepresentation(Value);

    public bool Equals(BookGenre? other) =>
        other is not null
        && CompareTo(other.Value);

    public override bool Equals(object? obj) => Equals(obj as BookGenre);

    public override int GetHashCode() =>
        GetLowerCertainRepresentation(Value).GetHashCode();

    private static string GetLowerCertainRepresentation(string value)
        => value
            .Trim()
            .Replace("_", string.Empty)
            .ToLowerInvariant();
}