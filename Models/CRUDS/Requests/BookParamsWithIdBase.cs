using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Models.CRUDS.Requests;

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