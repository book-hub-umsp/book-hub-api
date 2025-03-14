using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Models.Books.Content;

/// <summary>
/// Модель для создаваемого раздела.
/// </summary>
public sealed record class CreatingPartition
{
    public Id<Book> BookId { get; }

    public PartitionContent Content { get; }

    public CreatingPartition(
        Id<Book> bookId,
        PartitionContent content)
    {
        BookId = bookId ?? throw new ArgumentNullException(nameof(bookId));
        Content = content ?? throw new ArgumentNullException(nameof(content));
    }
}