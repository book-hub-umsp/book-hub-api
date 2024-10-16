﻿using BookHub.Models.Books;
using BookHub.Models.Users;
using System;

namespace BookHub.Models.Common;

/// <summary>
/// Одиночный линк к пользователю и книге из избранного.
/// </summary>
public sealed class UserFavouriteBookLink : IEquatable<UserFavouriteBookLink>
{
    public Id<User> UserId { get; }

    public Id<Book> BookId { get; }

    public DateTimeOffset AddingTime { get; }

    public UserFavouriteBookLink(
        Id<User> userId, 
        Id<Book> bookId)
    {
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        BookId = bookId ?? throw new ArgumentNullException(nameof(bookId));

        AddingTime = DateTimeOffset.UtcNow;
    }

    public bool Equals(UserFavouriteBookLink? other) =>
        other is not null
            ? UserId == other.UserId && BookId == other.BookId
            : false;
    public override int GetHashCode() => HashCode.Combine(UserId, BookId);
}