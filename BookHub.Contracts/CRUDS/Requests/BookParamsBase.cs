using System.ComponentModel;

using Newtonsoft.Json;

namespace BookHub.Contracts.CRUDS.Requests;

/// <summary>
/// Маркерная базовая модель параметров.
/// </summary>
[TypeConverter(typeof(CommandConverter))]
public abstract class BookParamsBase
{
    [JsonProperty("command_type", Required = Required.Always)]
    public required CommandType CommandType { get; init; }
}