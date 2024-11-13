using Newtonsoft.Json;

namespace BookHub.Contracts;

/// <summary>
/// Класс, отвечающий за ошибочный результат исполнения команды.
/// </summary>
public sealed class FailureCommandResultResponse
{
    [JsonProperty("failure_message", Required = Required.Always)]
    public required string FailureMessage { get; init; }

    public static FailureCommandResultResponse FromException(Exception exception) =>
        new()
        {
            FailureMessage = exception.Message,
        };
}