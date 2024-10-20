namespace BookHub.Storage.PostgreSQL.Models;

/// <summary>
/// Ключевое слов.
/// </summary>
public sealed class KeyWord
{
    public long Id { get; set; }

    public required string Content { get; set; }

    public HashSet<KeyWordLink> BooksLinks { get; set; } = null!;
}
