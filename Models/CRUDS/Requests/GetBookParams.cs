using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Models.CRUDS.Requests;

/// <summary>
/// Параметры запроса получения книги.
/// </summary>
public sealed class GetBookParams : BookParamsWithIdBase
{
    public GetBookParams(Id<Book> bookId) : base(bookId)
    {
    }
}