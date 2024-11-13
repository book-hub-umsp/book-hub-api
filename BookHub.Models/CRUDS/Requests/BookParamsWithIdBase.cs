using System;

using BookHub.Models.Books.Repository;

namespace BookHub.Models.CRUDS.Requests;

/// <summary>
/// Базовая модель параметров книги с идентификатором.
/// </summary>
public abstract class BookParamsWithIdBase : BookParamsBase
{
    public Id<Book> BookId { get; }

    protected BookParamsWithIdBase(Id<Book> bookId)
    {
        BookId = bookId ?? throw new ArgumentNullException(nameof(bookId));
    }
}