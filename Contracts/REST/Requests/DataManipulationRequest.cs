using BookHub.API.Contracts.REST.Filtration;
using BookHub.API.Contracts.REST.Pagination;
using BookHub.API.Models.API;

using Newtonsoft.Json;

namespace BookHub.API.Contracts.REST.Requests;

public sealed class DataManipulationRequest : IRequestModel<DataManipulation>
{
    [JsonConverter(typeof(PaggingConverter))]
    [JsonProperty("pagination")]
    public PaggingBase? Pagination { get; init; }

    [JsonProperty("filters")]
    public IReadOnlyCollection<Filter> Filters { get; init; } = [];

    public DataManipulation ToDomain()
    {
        return new(
            PaggingBase.ToDomain(Pagination),
            Filters.Select(x => x.ToDomain()).ToList() ?? []);
    }
}
