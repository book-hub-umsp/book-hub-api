using System.ComponentModel;
using System;

using BookHub.Models.Books;
using BookHub.Models.Users;

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
            throw new InvalidEnumArgumentException(
                nameof(newBookStatus),
                (int)newBookStatus,
                typeof(BookStatus));

        NewBookStatus = newBookStatus;
    }
}
