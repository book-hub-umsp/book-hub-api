namespace BookHub.API.Models.API.Pagination;

public sealed class WithoutPagination : IPagination
{
    public static WithoutPagination Instance { get; } = new();

    private WithoutPagination() { }
}
