using BookHub.API.Models.Books.Repository;

namespace BookHub.API.Models.CRUDS.Requests;

/// <summary>
/// Параметры запроса по обновлению параметров книги.
/// </summary>
public abstract class UpdateBookParamsBase : BookParamsWithIdBase
{
    protected UpdateBookParamsBase(Id<Book> bookId)
        : base(bookId)
    {
    }
}