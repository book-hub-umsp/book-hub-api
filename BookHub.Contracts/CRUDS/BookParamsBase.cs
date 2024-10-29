using Newtonsoft.Json;

namespace BookHub.Contracts.CRUDS;

/// <summary>
/// Маркерная базовая модель параметров.
/// </summary>
public abstract class BookParamsBase
{
    [JsonProperty("command_type", Required = Required.Always)]
    public required CommandType CommandType { get; init; }
}