namespace BookHub.Models.CRUDS.Requests;

/// <summary>
/// Модель запроса с заданием пагинации.
/// </summary>
public sealed class GetPaginedBooks
{
    public int PageNumber { get; }

    public int ElementsInPage { get; }

    public GetPaginedBooks(int pageNumber, int elementsInPage)
    {
        PageNumber = pageNumber;
        ElementsInPage = elementsInPage;
    }
}