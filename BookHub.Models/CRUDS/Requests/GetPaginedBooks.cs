namespace BookHub.Models.CRUDS.Requests;

/// <summary>
/// Модель запроса с заданием пагинации.
/// </summary>
public sealed class GetPaginedBooks : GetPaginedElementsBase
{
    public GetPaginedBooks(int pageNumber, int elementsInPage)
        : base(pageNumber, elementsInPage)
    {
    }
}