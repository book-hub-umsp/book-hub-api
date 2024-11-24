using System;

namespace BookHub.Models.API.Pagination;

/// <summary>
/// Постраничная пагинация.
/// </summary>
/// <remarks>
/// Использовать в тех случаях, когда нам необходима пагинация со случайным доступом к странице.
/// Работает медленне <see cref="OffsetPagination"/>, но позволяет запрашивать данные непоследовательно.
/// </remarks>
public sealed class PagePagination : PaginationBase
{
    public long ItemsTotal { get; }

    public int PagesTotal { get; }

    public int PageNumber { get; }

    public PagePagination(
        long itemsTotal,
        int pageNumber,
        int pageSize) : base(pageSize)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(itemsTotal, 0);
        ItemsTotal = itemsTotal;

        ArgumentOutOfRangeException.ThrowIfLessThan(pageNumber, 1);
        PageNumber = pageNumber;

        PagesTotal = ItemsTotal != 0
            ? (int)Math.Ceiling(itemsTotal / (double)pageSize)
            : 1;

        ArgumentOutOfRangeException.ThrowIfGreaterThan(pageNumber, PagesTotal);
    }
}
