using BookHub.Contracts.REST.Filtration;
using BookHub.Contracts.REST.Pagination;
using BookHub.Models.API;

using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Requests;

public sealed class DataManipulationRequest
{
    [JsonProperty("pagination")]
    public PaggingBase? Pagination { get; init; }

    [JsonProperty("filters")]
    public IReadOnlyCollection<Filter> Filters { get; init; } = [];

    public static DataManipulation ToDomain(DataManipulationRequest contract)
    {
        ArgumentNullException.ThrowIfNull(contract);

        return new(
            PaggingBase.ToDomain(contract.Pagination),
            contract.Filters.Select(Filter.ToDomain).ToList());
    }
}
