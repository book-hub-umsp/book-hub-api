using BookHub.API.Models.API.Filtration;

using Newtonsoft.Json;

namespace BookHub.API.Contracts.REST.Filtration;

public sealed class Filter : IRequestModel<FilterBase>
{
    [JsonProperty("type", Required = Required.Always)]
    public required FilterType Type { get; set; }

    [JsonProperty("field", Required = Required.Always)]
    public required string Field { get; set; }

    [JsonProperty("value")]
    public object? FilterValue { get; set; }

    public FilterBase ToDomain() =>
        Type switch
        {
            FilterType.Equals =>
                new EqualsFilter(Field, FilterValue!),

            FilterType.Contains =>
                new ContainsFilter(Field, FilterValue!),

            _ => throw new InvalidOperationException(
                $"Unknown filter type {Type}.")
        };
}
