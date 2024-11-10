namespace BookHub.Models.CRUDS.Requests;

/// <summary>
/// База для получения пагинированных элементов.
/// </summary>
public abstract class GetPaginedElementsBase
{
    public int PageNumber { get; }

    public int ElementsInPage { get; }

    protected GetPaginedElementsBase(int pageNumber, int elementsInPage)
    {
        PageNumber = pageNumber;
        ElementsInPage = elementsInPage;
    }
}