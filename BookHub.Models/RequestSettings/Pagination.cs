using System;

namespace BookHub.Models.RequestSettings;

/// <summary>
/// Настройки пагинации.
/// </summary>
public sealed record class Pagination
{
    public int ElementsTotal { get; }

    public int PagesTotal { get; }

    public int PageNumber { get; }

    public int ElementsInPage { get; }

    public int StartIndexNumber { get; }

    public int EndIndexNumber { get; }

    public Pagination(
        int elementsTotal, 
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

        PagesTotal = elementsTotal % elementsInPage == 0
            ? elementsTotal / elementsInPage
            : elementsTotal / elementsInPage + 1;

        ArgumentOutOfRangeException.ThrowIfGreaterThan(pageNumber, PagesTotal);

        ElementsTotal = elementsTotal;
        PageNumber = pageNumber;
        ElementsInPage = elementsInPage;

        StartIndexNumber = (pageNumber - 1) * elementsInPage;
        EndIndexNumber = StartIndexNumber + elementsInPage - 1;
    }

    private const int DEFAULT_ELEMENTS_IN_PAGE_REMINDER = 5;
}