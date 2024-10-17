using BookHub.Models;

namespace BookHub.Storage.PostgreSQL.Models;

public sealed class KeyWord
{
    public Id<KeyWord> Id { get; set; }

    public required string Content { get; set; }

    public HashSet<Book> Books { get; set; } = null!;
}
