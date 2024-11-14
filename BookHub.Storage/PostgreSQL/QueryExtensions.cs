using BookHub.Models.API.Pagination;
using BookHub.Storage.PostgreSQL.Models;

namespace BookHub.Storage.PostgreSQL;

/// <summary>
/// Экстеншены для запросов.
/// </summary>
public static class QueryExtensions
{
    public static IQueryable<T> WithPagging<T>(this IQueryable<T> items, PaginationBase pagination)
        where T : IKeyable
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(pagination);

        return pagination switch
        {
            PagePagination pagePagination => items
                .OrderBy(x => x.Id)
                .Skip(pagePagination.PageSize * (pagePagination.PageNumber - 1))
                .Take(pagePagination.PageSize),

            OffsetPagination offsetPagination => items
                .Where(x => x.Id > offsetPagination.Offset)
                .Take(offsetPagination.PageSize),

            _ => throw new InvalidOperationException(
                    $"Pagination type: {pagination.GetType().Name} is not supported.")
        };
    }
}