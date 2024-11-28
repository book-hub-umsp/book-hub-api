using System;

namespace BookHub.Models.API.Pagination;

/// <summary>
/// Постраничная пагинация.
/// </summary>
/// <remarks>
/// Использовать в тех случаях, когда нам необходима пагинация со случайным доступом к странице.
/// Работает медленне <see cref="OffsetPagination"/>, но позволяет запрашивать данные непоследовательно.
/// </remarks>
public sealed class PagePagination : IPagination
{
    public long ItemsTotal { get; }

    public int PagesTotal { get; }

    public PagePagging Pagging { get; }

    public PagePagination(
        PagePagging pagging,
        long itemsTotal)
    {
        ArgumentNullException.ThrowIfNull(pagging);
        Pagging = pagging;

        ArgumentOutOfRangeException.ThrowIfLessThan(itemsTotal, MIN_TOTAL_ITEMS);
        ItemsTotal = itemsTotal;

        PagesTotal = ItemsTotal != MIN_TOTAL_ITEMS
            ? (int)Math.Ceiling(itemsTotal / (double)Pagging.PageSize)
            : MIN_PAGES_TOTAL;
        ArgumentOutOfRangeException.ThrowIfGreaterThan(Pagging.PageNumber, PagesTotal);
    }

    private const long MIN_TOTAL_ITEMS = 0;
    private const int MIN_PAGES_TOTAL = 1;
}
