using System;

namespace BookHub.Models.API.Pagination;

/// <summary>
/// Базовая модель запроса на пагинацию.
/// </summary>
public abstract class PaggingBase
{
    public int PageSize { get; }

    protected PaggingBase(int pageSize)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, MIN_PAGE_SIZE);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(pageSize, MAX_PAGE_SIZE);

        PageSize = pageSize;
    }

    private const int MIN_PAGE_SIZE = 1;
    private const int MAX_PAGE_SIZE = 50;
}
