using BookHub.API.Models.Books.Repository;

namespace BookHub.API.Models.Books.Content;

/// <summary>
/// Модель для создаваемой главы.
/// </summary>
public sealed record class CreatingChapter
{
    public Id<Book> BookId { get; }

    public ChapterContent Content { get; }

    public CreatingChapter(
        Id<Book> bookId,
        ChapterContent content)
    {
        BookId = bookId ?? throw new ArgumentNullException(nameof(bookId));
        Content = content ?? throw new ArgumentNullException(nameof(content));
    }
}