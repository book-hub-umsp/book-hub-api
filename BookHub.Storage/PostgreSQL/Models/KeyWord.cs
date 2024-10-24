namespace BookHub.Storage.PostgreSQL.Models;

/// <summary>
/// Ключевое слово.
/// </summary>
public sealed class KeyWord
{
    public long Id { get; set; }

    public required string Content { get; set; }

    public ICollection<KeyWordLink> BooksLinks { get; set; } = null!;
}
