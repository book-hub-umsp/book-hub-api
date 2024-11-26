using BookHub.Contracts.REST.Filtration;
using BookHub.Contracts.REST.Pagination;
using BookHub.Models.API;

using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Requests;

public sealed class DataManipulationContainerRequest
{
    [JsonProperty("pagination")]
    public PaginationBase? Pagination { get; init; }

    [JsonProperty("filters")]
    public IReadOnlyCollection<Filter> Filters { get; init; } = [];

    public static DataManipulation ToDomain(DataManipulationContainerRequest contract)
    {
        ArgumentNullException.ThrowIfNull(contract);

        return new();
    }
}
