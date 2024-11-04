using System;

using BookHub.Models.Users;

namespace BookHub.Models.Books;

/// <summary>
/// Короткое описание книги.
/// </summary>
public sealed class BookPreview
{
    public Id<Book> Id { get; }

    public Name<Book> Title { get; }

    public BookGenre Genre { get; }

    public Id<User> AuthorId { get; }

    public BookPreview(
        Id<Book> id, 
        Name<Book> title, 
        BookGenre genre, 
        Id<User> authorId)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Genre = genre ?? throw new ArgumentNullException(nameof(genre));
        AuthorId = authorId ?? throw new ArgumentNullException(nameof(authorId));
    }
}
