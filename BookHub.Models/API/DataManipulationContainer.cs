using System;
using System.Collections.Generic;

using BookHub.Models.API.Filtration;
using BookHub.Models.API.Pagination;

namespace BookHub.Models.API;

/// <summary>
/// Представляет контейнер, содержащий модели манипуляции с данными.
/// </summary>
public sealed class DataManipulationContainer
{
    public PaginationBase Pagination { get; }

    public IReadOnlyCollection<FilterBase> Filters { get; }

    public DataManipulationContainer()
        : this(WithoutPagination.Instance, []) { }

    public DataManipulationContainer(PaginationBase pagination)
        : this(pagination, []) { }

    public DataManipulationContainer(IReadOnlyCollection<FilterBase> filters)
        : this(WithoutPagination.Instance, filters) { }

    public DataManipulationContainer(PaginationBase pagination, IReadOnlyCollection<FilterBase> filters)
    {
        ArgumentNullException.ThrowIfNull(pagination);
        Pagination = pagination;

        ArgumentNullException.ThrowIfNull(filters);
        Filters = filters;
    }
}
