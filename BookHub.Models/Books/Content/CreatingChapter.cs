using System;

using BookHub.Models.Books.Repository;

namespace BookHub.Models.Books.Content;

/// <summary>
/// Модель для создаваемой главы.
/// </summary>
public sealed class CreatingChapter
{
    public ChapterNumber ChapterNumber { get; }

    public Id<Book> BookId { get; }

    public ChapterContent Content { get; }

    public CreatingChapter(
        Id<Book> bookId, 
        ChapterContent content,
        ChapterNumber chapterNumber)
    {
        BookId = bookId ?? throw new ArgumentNullException(nameof(bookId));
        Content = content ?? throw new ArgumentNullException(nameof(content));
        ChapterNumber = chapterNumber ?? throw new ArgumentNullException(nameof(chapterNumber));
    }
}