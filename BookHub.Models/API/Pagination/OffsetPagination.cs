using System;

namespace BookHub.Models.API.Pagination;

/// <summary>
/// Пагинация по смещению.
/// </summary>
/// <remarks>
/// Использовать для пагинации с последовательным доступом к данным (предыдущий/следующий)
/// в ситуации, когда не требуется хаотичная выборка элементов.
/// </remarks>
public sealed class OffsetPagination : IPagination
{
    public OffsetPagging Pagging { get; }

    public OffsetPagination(OffsetPagging pagging)
    {
        ArgumentNullException.ThrowIfNull(pagging);
        Pagging = pagging;
    }
}
