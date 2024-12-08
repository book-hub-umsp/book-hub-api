using BookHub.Models.Books.Content;
using BookHub.Models;
using BookHub.Models.Books.Repository;
using BookHub.Storage.PostgreSQL.Abstractions.Converters;
using BookHub.Storage.PostgreSQL.Models.Helpers;

namespace BookHub.Storage.PostgreSQL.Converters;

/// <summary>
/// Представляет конвертера модели превью книги.
/// </summary>
public sealed class BookPreviewConverter : IBookPreviewConverter
{
    /// <inheritdoc/>
    public BookPreview ToDomain(StorageBookPreviewHelper storageHelper)
    {
        ArgumentNullException.ThrowIfNull(storageHelper);

        return new(
            new(storageHelper.BookId),
            new(storageHelper.Title),
            new(storageHelper.BookGenre.Value),
            new(storageHelper.AuthorId),
            storageHelper.Chapters
                .ToDictionary(
                    k => new Id<Chapter>(k.ChapterId),
                    v => new ChapterSequenceNumber(v.SequenceNumber)));
    }
}