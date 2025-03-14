using BookHub.API.Models.Books.Content;
using BookHub.API.Models.Books.Repository;

namespace BookHub.API.Storage.PostgreSQL.Models.Previews;

/// <summary>
/// Вспомогательная модель превью книги из хранилища.
/// </summary>
public sealed class StorageBookPreview
{
    public required long BookId { get; init; }

    public required string Title { get; init; }

    public required long AuthorId { get; init; }

    public required BookGenre BookGenre { get; init; }

    public ICollection<int> Partitions { get; init; } = null!;

    public static BookPreview ToDomain(StorageBookPreview storagePreview)
    {
        ArgumentNullException.ThrowIfNull(storagePreview);

        return new(
            new(storagePreview.BookId),
            new(storagePreview.Title),
            new(storagePreview.BookGenre.Value),
            new(storagePreview.AuthorId),
            storagePreview.Partitions
                .Select(x => new PartitionSequenceNumber(x))
                .ToHashSet());
    }
}