using System;

using BookHub.Models.Books.Repository;

namespace BookHub.Models.Books.Content;

/// <summary>
/// Модель главы книги.
/// </summary>
public sealed class Chapter
{
    public Id<Chapter> Id { get; }

    public Name<Chapter> Name { get; }

    public ChapterNumber ChapterNumber { get; }

    public Id<Book> BookId { get; }

    public Content Content { get; }

    public Chapter(
        Id<Chapter> id,
        Name<Chapter> name,
        ChapterNumber chapterNumber,
        Id<Book> bookId,
        Content content)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        ChapterNumber = chapterNumber ?? throw new ArgumentNullException(nameof(chapterNumber));
        BookId = bookId ?? throw new ArgumentNullException(nameof(bookId));
        Content = content ?? throw new ArgumentNullException(nameof(content));
    }
}