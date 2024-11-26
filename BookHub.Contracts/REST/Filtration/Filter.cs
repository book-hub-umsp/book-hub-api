using BookHub.Models.API.Filtration;

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

    public static FilterBase ToDomain(Filter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        return filter.Type switch
        {
            FilterType.Equals =>
                new EqualsFilter(filter.PropertyName, filter.FilterValue!),

            FilterType.Contains =>
                new ContainsFilter(filter.PropertyName, filter.FilterValue!),

            _ => throw new InvalidOperationException(
                $"Unknown filter type {filter.Type}.")
        };
    }
}
