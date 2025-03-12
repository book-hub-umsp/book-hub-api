using BookHub.API.Models;
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

    public required BookGenre BookGenre { get; init; }

    public required long AuthorId { get; init; }

    public ICollection<StorageChapterPreview> Chapters { get; init; } = null!;

    public static BookPreview ToDomain(StorageBookPreview storagePreview)
    {
        ArgumentNullException.ThrowIfNull(storagePreview);

        return new(
            new(storagePreview.BookId),
            new(storagePreview.Title),
            new(storagePreview.BookGenre.Value),
            new(storagePreview.AuthorId),
            storagePreview.Chapters
                .ToDictionary(
                    k => new Id<API.Models.Books.Content.Chapter>(k.ChapterId),
                    v => new ChapterSequenceNumber(v.SequenceNumber)));
    }
}