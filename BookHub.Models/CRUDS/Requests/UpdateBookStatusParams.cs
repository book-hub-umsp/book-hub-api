﻿using System;
using System.ComponentModel;

using BookHub.Models.Account;
using BookHub.Models.Books.Repository;

namespace BookHub.Models.CRUDS.Requests;

/// <summary>
/// Параметры запроса по обновлению статуса книги.
/// </summary>
public sealed class UpdateBookStatusParams : UpdateBookParamsBase
{
    public BookStatus NewBookStatus { get; }

    public UpdateBookStatusParams(
        Id<Book> bookId,
        Id<User> authorId,
        BookStatus newBookStatus)
        : base(bookId, authorId)
    {
        if (!Enum.IsDefined(newBookStatus))
        {
            throw new InvalidEnumArgumentException(
                nameof(newBookStatus),
                (int)newBookStatus,
                typeof(BookStatus));
        }

        NewBookStatus = newBookStatus;
    }
}
