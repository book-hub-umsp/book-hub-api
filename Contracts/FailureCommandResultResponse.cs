﻿using Newtonsoft.Json;

namespace BookHub.API.Contracts;

/// <summary>
/// Класс, отвечающий за ошибочный результат исполнения команды.
/// </summary>
public sealed class FailureCommandResultResponse
{
    [JsonProperty("failure_message", Required = Required.Always)]
    public required string FailureMessage { get; init; }

    public static FailureCommandResultResponse FromException(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        return new()
        {
            FailureMessage = exception.Message,
        };
    }
}