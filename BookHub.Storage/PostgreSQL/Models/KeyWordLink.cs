using BookHub.Models;
using BookHub.Models.Books;

namespace BookHub.Storage.PostgreSQL.Models;

/// <summary>
/// Ссылки на ключевые слова.
/// </summary>
public sealed class KeyWordLink
{
    public required long KeyWordId { get; init; }

    public KeyWord KeyWord { get; set; }

    public required long BookId { get; init; }

    public Book Book { get; set; }
}