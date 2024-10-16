﻿namespace BookHub.Storage.Models;

/// <summary>
/// Модель привязки к автору книги.
/// </summary>
public sealed class BookAuthor : IEquatable<BookAuthor>
{
    public Id<User> AuthorId { get; }

    public Name<User> AuthorName { get; }

    public bool Equals(BookAuthor? other) => 
        other is not null
            ? AuthorId == other.AuthorId 
            : false;
}
