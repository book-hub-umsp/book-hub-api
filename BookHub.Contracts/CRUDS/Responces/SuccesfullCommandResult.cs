using Newtonsoft.Json;

namespace BookHub.Contracts.CRUDS.Responces;

/// <summary>
/// Класс, отвечающий за успешный результат исполнения команды.
/// </summary>
public sealed class SuccesfullCommandResult : CommandExecutionResultBase
{
    [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
    public object? Content { get; init; }
}