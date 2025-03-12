using BookHub.API.Models.Books.Repository;

namespace BookHub.API.Models.Books.Content;

/// <summary>
/// Модель главы книги.
/// </summary>
public sealed class Chapter
{
    public Id<Chapter> Id { get; }

    public ChapterSequenceNumber ChapterNumber { get; }

    public Id<Book> BookId { get; }

    public ChapterContent Content { get; }

    public Chapter(
        Id<Chapter> id,
        ChapterSequenceNumber chapterNumber,
        Id<Book> bookId,
        ChapterContent content)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        ChapterNumber = chapterNumber ?? throw new ArgumentNullException(nameof(chapterNumber));
        BookId = bookId ?? throw new ArgumentNullException(nameof(bookId));
        Content = content ?? throw new ArgumentNullException(nameof(content));
    }
}