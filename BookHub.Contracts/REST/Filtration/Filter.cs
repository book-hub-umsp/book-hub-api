using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Filtration;

public sealed class Filter
{
    [JsonProperty("type", Required = Required.Always)]
    public required FilterType Type { get; set; }

    [JsonProperty("field", Required = Required.Always)]
    public required string PropertyName { get; set; }

    [JsonProperty("value")]
    public object? FilterValue { get; set; }
}
