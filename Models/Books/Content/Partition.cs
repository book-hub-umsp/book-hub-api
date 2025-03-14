using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Models.Books.Content;

/// <summary>
/// Модель раздела книги.
/// </summary>
public sealed class Partition
{
    public Id<Book> BookId { get; }

    public PartitionSequenceNumber PartitionNumber { get; }

    public PartitionContent Content { get; }

    public Partition(
        Id<Book> bookId,
        PartitionSequenceNumber partitionNumber,
        PartitionContent content)
    {
        ArgumentNullException.ThrowIfNull(bookId);
        BookId = bookId;

        ArgumentNullException.ThrowIfNull(partitionNumber);
        PartitionNumber = partitionNumber;

        ArgumentNullException.ThrowIfNull(content);
        Content = content;
    }
}