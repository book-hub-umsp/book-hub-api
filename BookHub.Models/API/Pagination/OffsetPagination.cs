using System;

namespace BookHub.Models.API.Pagination;

/// <summary>
/// Пагинация по смещению.
/// </summary>
/// <remarks>
/// Использовать для пагинации с последовательным доступом к данным (предыдущий/следующий)
/// в ситуации, когда не требуется хаточная выборка элементов.
/// </remarks>
public sealed class OffsetPagination : PaginationBase
{
    public long Offset { get; }

    public OffsetPagination(long offset, int pageSize) : base(pageSize)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(offset, MIN_OFFSET);
        Offset = offset;
    }

    private const long MIN_OFFSET = 0;
}
