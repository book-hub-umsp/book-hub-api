namespace BookHub.Models.API.Pagination;

/// <summary>
/// Модель означающая отсутствие пагинации.
/// </summary>
public sealed class WithoutPagging : PaggingBase
{
    /// <summary>
    /// Экземпляр.
    /// </summary>
    public static WithoutPagging Instance { get; } = new(DEFAULT_PAGE_SIZE);

    private WithoutPagging(int pageSize) : base(pageSize)
    {
    }

    private const int DEFAULT_PAGE_SIZE = 1;
}
