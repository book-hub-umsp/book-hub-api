using Newtonsoft.Json;

namespace BookHub.Contracts;

/// <summary>
/// Транспортная модель ключевого слова.
/// </summary>
public sealed class KeyWord
{
    [JsonProperty("content", Required = Required.Always)]
    public string Content { get; set; }
}