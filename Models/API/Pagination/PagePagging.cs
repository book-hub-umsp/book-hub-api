namespace BookHub.API.Models.API.Pagination;

/// <summary>
/// Модель запроса на пагинацию для <see cref="PagePagination"/>.
/// </summary>
public sealed class PagePagging : PaggingBase
{
    public int PagesTotal { get; }

    public int PageNumber { get; }

    public PagePagging(
        int pageNumber,
        int pageSize) : base(pageSize)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(pageNumber, MIN_PAGE_NUMBER);
        PageNumber = pageNumber;
    }

    private const int MIN_PAGE_NUMBER = 1;
}
