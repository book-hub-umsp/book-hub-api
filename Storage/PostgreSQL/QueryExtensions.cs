using System.Linq.Dynamic.Core;
using System.Text;

using BookHub.API.Models.API.Filtration;
using BookHub.API.Models.API.Pagination;
using BookHub.API.Storage.PostgreSQL.Models;
using BookHub.API.Storage.PostgreSQL.Models.Previews;

using Microsoft.EntityFrameworkCore;

namespace BookHub.API.Storage.PostgreSQL;

/// <summary>
/// Экстеншены для запросов.
/// </summary>
public static class QueryExtensions
{
    public static IQueryable<T> WithPaging<T>(
        this IQueryable<T> items,
        PaggingBase pagination)
        where T : IKeyable
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(pagination);

        return pagination switch
        {
            WithoutPagging withoutPagination => items,

            PagePagging pagePagination => items
                .OrderBy(x => x.Id)
                .Skip(pagePagination.PageSize * (pagePagination.PageNumber - 1))
                .Take(pagePagination.PageSize),

            OffsetPagging offsetPagination => items
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

    private static (string, object[]) BuildFiltersQuery(
        IReadOnlyCollection<FilterBase> filters)
    {
        var count = 0;
        var sb = new StringBuilder();
        var parameters = new object[filters.Count];

        sb.Append($"x => {GetQueryFromFilter(filters.First(), parameters, ref count)}");
        foreach (var filter in filters.Skip(1))
        {
            sb.Append($" && {GetQueryFromFilter(filter, parameters, ref count)}");
        }

        return (sb.ToString(), parameters);
    }

    private static string GetQueryFromFilter(
        FilterBase filter,
        object[] parameters,
        ref int counter)
    {
        switch (filter)
        {
            case EqualsFilter equalsFilter:
                parameters[counter] = equalsFilter.Value;
                return $"x.{equalsFilter.PropertyName} == @{counter++}";

            case ContainsFilter containsFilter:
                parameters[counter] = containsFilter.Value;
                return $"x.{containsFilter.PropertyName}.Contains(@{counter++})";

            default:
                throw new InvalidOperationException(
                    $"Filter type {filter.GetType().Name} not supported.");
        }
    }
}