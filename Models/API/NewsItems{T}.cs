using System.Collections;

using BookHub.API.Models.API.Pagination;

namespace BookHub.API.Models.API;

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
    public IPagination Pagination { get; }

    public NewsItems(IReadOnlyCollection<T> items)
        : this(items, WithoutPagination.Instance) { }

    public NewsItems(IReadOnlyCollection<T> items, IPagination pagination)
    {
        ArgumentNullException.ThrowIfNull(items);
        Items = items;

        ArgumentNullException.ThrowIfNull(pagination);
        Pagination = pagination;
    }

    public IEnumerator<T> GetEnumerator() => Items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
}
