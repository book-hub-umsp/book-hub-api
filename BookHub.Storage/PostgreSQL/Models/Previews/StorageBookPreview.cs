using BookHub.Models.Books.Content;
using BookHub.Models;
using BookHub.Models.Books.Repository;

namespace BookHub.Storage.PostgreSQL.Models.Helpers;

/// <summary>
/// Вспомогательная модель превью книги из хранилища.
/// </summary>
public sealed class StorageBookPreview
{
    public required long BookId { get; init; }

    public required string Title { get; init; }

    public required BookGenre BookGenre { get; init; }

    public required long AuthorId { get; init; }

    public IEnumerable<StorageChapterPreview> Chapters { get; init; } = [];

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
                    k => new Id<BookHub.Models.Books.Content.Chapter>(k.ChapterId),
                    v => new ChapterSequenceNumber(v.SequenceNumber)));
    }
}