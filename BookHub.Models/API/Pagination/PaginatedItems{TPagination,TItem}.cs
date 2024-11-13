using System;
using System.Collections;
using System.Collections.Generic;

namespace BookHub.Models.API.Pagination;

/// <summary>
/// Пагинированные элементы.
/// </summary>
/// <typeparam name="TPagination">
/// Тип пагенации, примененный к элементам.
/// </typeparam>
/// <typeparam name="TItem">
/// Коллекция пагинированных элементов.
/// </typeparam>
public sealed class PaginatedItems<TPagination, TItem> : IEnumerable<TItem>
    where TPagination : PaginationBase
{
    public TPagination Pagination { get; }

    public IReadOnlyCollection<TItem> Items { get; }

    public PaginatedItems(TPagination pagination, IReadOnlyCollection<TItem> items)
    {
        ArgumentNullException.ThrowIfNull(pagination);
        Pagination = pagination;

        ArgumentNullException.ThrowIfNull(items);
        Items = items;
    }

    public IEnumerator<TItem> GetEnumerator() => Items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
}
