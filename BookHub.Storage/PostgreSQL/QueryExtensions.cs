using System.Linq.Dynamic.Core;
using System.Text;

using BookHub.Models.API.Filtration;
using BookHub.Models.API.Pagination;
using BookHub.Storage.PostgreSQL.Models;

namespace BookHub.Storage.PostgreSQL;

/// <summary>
/// Экстеншены для запросов.
/// </summary>
public static class QueryExtensions
{
    public static IQueryable<T> WithPaging<T>(this IQueryable<T> items, PaginationBase pagination)
        where T : IKeyable
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(pagination);

        return pagination switch
        {
            WithoutPagination withoutPagination => items,

            PagePagination pagePagination => items
                .OrderBy(x => x.Id)
                .Skip(pagePagination.PageSize * (pagePagination.PageNumber - 1))
                .Take(pagePagination.PageSize),

            OffsetPagination offsetPagination => items
                .OrderBy(x => x.Id)
                .Where(x => x.Id > offsetPagination.Offset)
                .Take(offsetPagination.PageSize),

            _ => throw new InvalidOperationException(
                    $"Pagination type: {pagination.GetType().Name} is not supported.")
        };
    }

    public static IQueryable<T> WithFiltering<T>(
        this IQueryable<T> items,
        IReadOnlyCollection<FilterBase> filters)
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(filters);

        if (!filters.Any())
        {
            return items;
        }

        var (query, parameters) = BuildFiltersQuery(filters);

        return items.Where(query, parameters);
    }

    private static FiltersQuery BuildFiltersQuery(IReadOnlyCollection<FilterBase> filters)
    {
        var count = 0;
        var sb = new StringBuilder();
        var parameters = new List<object>(filters.Count);

        sb.Append($"x => {BuildQueryFromFilter(filters.First(), parameters, ref count)}");
        foreach (var filter in filters.Skip(1))
        {
            sb.Append($" && {BuildQueryFromFilter(filter, parameters, ref count)}");
        }

        return new(sb.ToString(), parameters.ToArray());
    }

    private static string BuildQueryFromFilter(
        FilterBase filter,
        List<object> parameters,
        ref int counter)
    {
        if (filter is EqualsFilter equalsFilter)
        {
            parameters.Add(equalsFilter.Value);

            return $"x.{equalsFilter.PropertyName} == @{counter++}";
        }

        if (filter is ContainsFilter containsFilter)
        {
            parameters.Add(containsFilter.Value);

            return $"x.{containsFilter.PropertyName}.Contains(@{counter++})";
        }

        throw new InvalidOperationException($"Filter type {filter.GetType().Name} not supported");
    }

    private readonly record struct FiltersQuery(string Query, object[] Parameters);
}