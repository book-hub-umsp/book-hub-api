using BookHub.Models;

namespace BookHub.Storage.PostgreSQL.Models;

public sealed class KeyWord
{
    public long Id { get; set; }

    public required string Content { get; set; }

    public HashSet<KeyWordLink> BooksLinks { get; set; } = null!;
}
