using System;
using System.Collections.Generic;

using BookHub.Models.API.Filtration;
using BookHub.Models.API.Pagination;

namespace BookHub.Models.API;

/// <summary>
/// Представляет контейнер, содержащий модели манипуляции с данными.
/// </summary>
public sealed class DataManipulation
{
    public PaggingBase Pagination { get; }

    public IReadOnlyCollection<FilterBase> Filters { get; }

    public DataManipulation()
        : this(WithoutPagging.Instance, []) { }

    public DataManipulation(PaggingBase pagination)
        : this(pagination, []) { }

    public DataManipulation(IReadOnlyCollection<FilterBase> filters)
        : this(WithoutPagging.Instance, filters) { }

    public DataManipulation(PaggingBase pagination, IReadOnlyCollection<FilterBase> filters)
    {
        ArgumentNullException.ThrowIfNull(pagination);
        Pagination = pagination;

        ArgumentNullException.ThrowIfNull(filters);
        Filters = filters;
    }
}
