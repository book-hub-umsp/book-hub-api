namespace BookHub.API.Storage.PostgreSQL.Models;

/// <summary>
/// Ссылка между книгой и ключевым словом.
/// </summary>
public sealed class KeywordLink
{
    public long KeywordId { get; set; }

    public Keyword Keyword { get; set; } = null!;

    public long BookId { get; set; }

    public Book Book { get; set; } = null!;
}