using BookHub.Models.Users;
using System;

namespace BookHub.Models.Books;

/// <summary>
/// Модель привязки к автору книги.
/// </summary>
public sealed class BookAuthor : IEquatable<BookAuthor>
{
    public Id<User> AuthorId { get; }

    public Name<User> AuthorName { get; }

    public BookAuthor(
        Id<User> authorId, 
        Name<User> authorName)
    {
        AuthorId = authorId ?? throw new ArgumentNullException(nameof(authorId));
        AuthorName = authorName ?? throw new ArgumentNullException(nameof(authorName));
    }

    public bool Equals(BookAuthor? other) => 
        other is not null
            ? AuthorId == other.AuthorId 
            : false;
    public override int GetHashCode() => AuthorId.GetHashCode();
}
