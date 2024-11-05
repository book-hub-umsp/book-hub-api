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
        ArgumentOutOfRangeException.ThrowIfLessThan(elementsInPage, 1);

        if (elementsInPage % DEFAULT_ELEMENTS_IN_PAGE_REMINDER != 0)
        {
            throw new ArgumentException(
                $"Inconsitent elements in page number" +
                $" not satisfied to hardcoded reminder {DEFAULT_ELEMENTS_IN_PAGE_REMINDER}.");
        }

        PagesTotal = (int)Math.Ceiling(elementsTotal / (double)elementsInPage);

        ArgumentOutOfRangeException.ThrowIfGreaterThan(pageNumber, PagesTotal);

        ElementsTotal = elementsTotal;
        PageNumber = pageNumber;
        ElementsInPage = elementsInPage;
    }

    private const int DEFAULT_ELEMENTS_IN_PAGE_REMINDER = 5;
}