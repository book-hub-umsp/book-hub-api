using System;

using BookHub.Models.Books;

namespace BookHub.Models.CRUDS;

/// <summary>
/// Базовая модель параметров книги с идентификатором.
/// </summary>
public abstract class BookParamsWithIdBase
{
    public Id<Book> BookId { get; }

    protected BookParamsWithIdBase(Id<Book> bookId)
    {
        BookId = bookId ?? throw new ArgumentNullException(nameof(bookId));
    }
}
