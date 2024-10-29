using BookHub.Models.Books;

namespace BookHub.Models.CRUDS;

/// <summary>
/// Параметры запроса получения книги.
/// </summary>
public sealed class GetBookParams : BookParamsWithIdBase
{
    public GetBookParams(Id<Book> bookId) : base(bookId)
    {
    }
}