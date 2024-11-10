using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Responces;

/// <summary>
/// Класс, отвечающий за успешный результат исполнения команды.
/// </summary>
public sealed class SuccesfullCommandResultResponse : CommandExecutionResultBase
{
    [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
    public object? Content { get; init; }
}