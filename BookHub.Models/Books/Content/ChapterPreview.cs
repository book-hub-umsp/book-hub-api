using System;

using BookHub.Models.Books.Repository;

namespace BookHub.Models.Books.Content;

/// <summary>
/// Модель краткого представления главы.
/// </summary>
public sealed class ChapterPreview
{
    public Id<Chapter> Id { get; }

    public Id<Book> BookId { get; }

    public ChapterNumber ChapterNumber { get; }

    public Name<Chapter> Title { get; }

    public ChapterPreview(
        Id<Chapter> id,
        Id<Book> bookId,
        ChapterNumber chapterNumber,
        Name<Chapter> title)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        BookId = bookId ?? throw new ArgumentNullException(nameof(bookId));
        ChapterNumber = chapterNumber ?? throw new ArgumentNullException(nameof(chapterNumber));
        Title = title ?? throw new ArgumentNullException(nameof(title));
    }
}