using Newtonsoft.Json;

namespace BookHub.Contracts.CRUDS.Responces;

/// <summary>
/// Класс, отвечающий за ошибочный результат исполнения команды.
/// </summary>
public sealed class FailureCommandResult : CommandExecutionResultBase
{
    [JsonProperty("failure_message", Required = Required.Always)]
    public required string FailureMessage { get; init; }
}