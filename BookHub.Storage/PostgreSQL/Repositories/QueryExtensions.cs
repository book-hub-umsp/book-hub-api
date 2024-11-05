namespace BookHub.Storage.PostgreSQL.Repositories;

/// <summary>
/// Экстеншены для запросов.
/// </summary>
public static class QueryExtensions
{
    public static IQueryable<T> GetPageElements<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize)
    {
        if (pageNumber-- <= 0)
        {
            throw new ArgumentException("PageNumber should be greater than 0.");
        }

        return query.Skip(pageSize * pageNumber).Take(pageSize);
    }
}