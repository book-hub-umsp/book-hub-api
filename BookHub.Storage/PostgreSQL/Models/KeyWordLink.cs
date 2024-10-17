using BookHub.Models;

namespace BookHub.Storage.PostgreSQL.Models;

/// <summary>
/// Ссылки на ключевые слова.
/// </summary>
public sealed class KeyWordLink
{
    public required Id<KeyWord> KeyWordId { get; init; }

    public required Id<Book> BookId { get; init; }
}