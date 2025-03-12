using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.Identifiers;

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