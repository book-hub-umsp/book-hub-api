using System;
using System.Globalization;
using System.Text;

using BookHub.Models.Books.Repository;

namespace BookHub.Models.Books.Content;

/// <summary>
/// Модель для создаваемой главы.
/// </summary>
public sealed class CreatingChapter
{
    public Name<Chapter> Title { get; }

    public ChapterNumber ChapterNumber { get; }

    public Id<Book> BookId { get; }

    public Content Content { get; }

    public CreatingChapter(
        Id<Book> bookId, 
        Content content,
        ChapterNumber chapterNumber,
        Name<Chapter>? title = null)
    {
        BookId = bookId ?? throw new ArgumentNullException(nameof(bookId));
        Content = content ?? throw new ArgumentNullException(nameof(content));
        ChapterNumber = chapterNumber ?? throw new ArgumentNullException(nameof(chapterNumber));

        Title = title is not null 
            ? title 
            : new Name<Chapter>(
                string.Format(
                    CultureInfo.InvariantCulture, 
                    _chapterNameFormat, 
                    ChapterNumber.Value));
    }

    private static readonly CompositeFormat _chapterNameFormat =
        CompositeFormat.Parse("Chapter {0}");
}