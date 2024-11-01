﻿using BookHub.Models.Books;
using BookHub.Models.Users;
using System;

namespace BookHub.Models;

/// <summary>
/// Одиночный линк к пользователю и книге из избранного.
/// </summary>
public sealed record UserFavoriteBookLink
{
    public Id<User> UserId { get; }

    public Id<Book> BookId { get; }


    public UserFavoriteBookLink(
        Id<User> userId,
        Id<Book> bookId)
    {
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));
        BookId = bookId ?? throw new ArgumentNullException(nameof(bookId));
    }
}