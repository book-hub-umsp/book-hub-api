using System;

namespace BookHub.Models.RequestSettings;

/// <summary>
/// Настройки пагинации.
/// </summary>
public sealed record class Pagination
{
    public long ElementsTotal { get; }

    public int PagesTotal { get; }

    public int PageNumber { get; }

    public int ElementsInPage { get; }

    public Pagination(
        long elementsTotal, 
        int pageNumber, 
        int elementsInPage)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(elementsTotal, 0);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageNumber, 1);

        ArgumentOutOfRangeException.ThrowIfLessThan(elementsInPage, ELEMENTS_IN_PAGE_MIN);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(elementsInPage, ELEMENTS_IN_PAGE_MAX);

        PagesTotal = (int)Math.Ceiling(elementsTotal / (double)elementsInPage);

        ArgumentOutOfRangeException.ThrowIfGreaterThan(pageNumber, PagesTotal);

        ElementsTotal = elementsTotal;
        PageNumber = pageNumber;
        ElementsInPage = elementsInPage;
    }

    private const int ELEMENTS_IN_PAGE_MIN = 1;
    private const int ELEMENTS_IN_PAGE_MAX = 50;
}