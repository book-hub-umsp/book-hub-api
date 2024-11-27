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

    public IReadOnlyDictionary<long, ChapterNumber> ChaptersIdsMap { get; }

    public BookPreview(
        Id<Book> id,
        Name<Book> title,
        BookGenre genre,
        Id<User> authorId,
        IReadOnlyDictionary<long, ChapterNumber> chaptersIdsMap)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Genre = genre ?? throw new ArgumentNullException(nameof(genre));
        AuthorId = authorId ?? throw new ArgumentNullException(nameof(authorId));
        ChaptersIdsMap = chaptersIdsMap 
            ?? throw new ArgumentNullException(nameof(chaptersIdsMap));
    }
}