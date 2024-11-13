namespace BookHub.Models.API.Pagination;

/// <summary>
/// Модель запроса на пагинацию для <see cref="PagePagination"/>.
/// </summary>
/// <param name="Page">
/// Запрашиваемая страница.
/// </param>
/// <param name="PageSize">
/// Размер страницы.
/// </param>
public sealed record class PagePagging(int Page, int PageSize);
