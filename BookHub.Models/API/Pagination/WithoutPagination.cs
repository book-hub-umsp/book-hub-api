namespace BookHub.Models.API.Pagination;

/// <summary>
/// Модель означающая отсутствие пагинации.
/// </summary>
public sealed class WithoutPagination : PaginationBase
{
    /// <summary>
    /// Экземпляр.
    /// </summary>
    public static WithoutPagination Instance { get; } = new(DEFAULT_PAGE_SIZE);

    private WithoutPagination(int pageSize) : base(pageSize)
    {
    }

    private const int DEFAULT_PAGE_SIZE = 1;
}
