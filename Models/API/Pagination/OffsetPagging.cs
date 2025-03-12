namespace BookHub.API.Models.API.Pagination;

/// <summary>
/// Модель запроса на пагинацию для <see cref="OffsetPagination"/>.
/// </summary>
public sealed class OffsetPagging : PaggingBase
{
    public long Offset { get; }

    public OffsetPagging(long offset, int pageSize) : base(pageSize)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(offset, MIN_OFFSET);
        Offset = offset;
    }

    private const long MIN_OFFSET = 0;
}
