using System.ComponentModel;
using System;

using BookHub.Models.Books;

namespace BookHub.Models.CRUDS;

/// <summary>
/// Параметры запроса по обновлению статуса книги.
/// </summary>
public sealed class UpdateBookStatusParams : UpdateBookParamsBase
{
    public BookStatus NewBookStatus { get; }

    public UpdateBookStatusParams(
        Id<Book> bookId, 
        BookStatus newBookStatus)
        : base(bookId)
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
