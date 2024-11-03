namespace BookHub.Models.CRUDS.Requests;

/// <summary>
/// Модель запроса с заданием пагинации.
/// </summary>
public sealed class GetPaginedBooks
{
    public int ElementsTotal { get; }

    public int PageNumber { get; }

    public int ElementsInPage { get; }

    public GetPaginedBooks(int elementsTotal, int pageNumber, int elementsInPage)
    {
        ElementsTotal = elementsTotal;
        PageNumber = pageNumber;
        ElementsInPage = elementsInPage;
    }
}