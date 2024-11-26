using System;
using System.Collections.Generic;

using BookHub.Models.Account;
using BookHub.Models.Books.Content;

namespace BookHub.Models.Books.Repository;

/// <summary>
/// Короткое описание книги.
/// </summary>
public sealed class BookPreview
{
    public Id<Book> Id { get; }

    public Name<Book> Title { get; }

    public BookGenre Genre { get; }

    public Id<User> AuthorId { get; }

    public IReadOnlyList<ChapterNumber> ChapterNumbers { get; }

    public BookPreview(
        Id<Book> id,
        Name<Book> title,
        BookGenre genre,
        Id<User> authorId,
        IReadOnlyList<ChapterNumber> chapterNumbers)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Genre = genre ?? throw new ArgumentNullException(nameof(genre));
        AuthorId = authorId ?? throw new ArgumentNullException(nameof(authorId));
        ChapterNumbers = chapterNumbers 
            ?? throw new ArgumentNullException(nameof(chapterNumbers));
    }
}
