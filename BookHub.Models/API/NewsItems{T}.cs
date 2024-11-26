using System;
using System.Collections;
using System.Collections.Generic;

using BookHub.Models.API.Pagination;

namespace BookHub.Models.API;

/// <summary>
/// Новостные элементы.
/// </summary>
/// <typeparam name="T">
/// Тип элемента, представленный в новости.
/// </typeparam>
public sealed class NewsItems<T> : IEnumerable<T>
{
    /// <summary>
    /// Элементы.
    /// </summary>
    public IReadOnlyCollection<T> Items { get; }

    /// <summary>
    /// Пагинация.
    /// </summary>
    public PaginationBase Pagination { get; }

    public NewsItems(IReadOnlyCollection<T> items)
        : this(items, WithoutPagging.Instance) { }

    public NewsItems(IReadOnlyCollection<T> items, PaginationBase pagination)
    {
        ArgumentNullException.ThrowIfNull(items);
        Items = items;

        ArgumentNullException.ThrowIfNull(pagination);
        Pagination = pagination;
    }

    public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
}
