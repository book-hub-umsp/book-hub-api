namespace BookHub.API.Models.Books.Repository;

/// <summary>
/// Жанр книги.
/// </summary>
public sealed class BookGenre
{
    public string Value { get; }

    public BookGenre(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        Value = value.Trim().ToLowerInvariant();
    }
}