using BookHub.API.Contracts.REST.Filtration;
using BookHub.API.Contracts.REST.Pagination;
using BookHub.API.Models.API;

using Newtonsoft.Json;

namespace BookHub.API.Contracts.REST.Requests;

public sealed class DataManipulationRequest
{
    [JsonConverter(typeof(PaggingConverter))]
    [JsonProperty("pagination")]
    public PaggingBase? Pagination { get; init; }

    [JsonProperty("filters")]
    public IReadOnlyCollection<Filter> Filters { get; init; } = [];

    public static DataManipulation ToDomain(DataManipulationRequest? contract)
    {
        return new(
            PaggingBase.ToDomain(contract?.Pagination),
            contract?.Filters.Select(Filter.ToDomain).ToList() ?? []);
    }
}
