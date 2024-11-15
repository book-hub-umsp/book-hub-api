namespace BookHub.Models.API.Pagination;

/// <summary>
/// Модель запроса на пагинацию для <see cref="OffsetPagination"/>.
/// </summary>
/// <param name="Offset">
/// Смещение.
/// </param>
/// <param name="PageSize">
/// Размер страницы.
/// </param>
public sealed record class OffsetPagging(long Offset, int PageSize);
