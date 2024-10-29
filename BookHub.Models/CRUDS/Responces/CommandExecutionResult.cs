using BookHub.Models.Books;
using System.ComponentModel;
using System;

namespace BookHub.Models.CRUDS.Responces;

/// <summary>
/// Класс, отвечающий за результат исполнения команды.
/// </summary>
public class CommandExecutionResult
{
    public CommandResult CommandResult { get; }

    public string? FailureMessage { get; }

    public CommandExecutionResult(
        CommandResult commandResult, 
        string? failureMessage = null)
    {
        if (!Enum.IsDefined(commandResult))
            throw new InvalidEnumArgumentException(
                nameof(commandResult),
                (int)commandResult,
                typeof(CommandResult));

        FailureMessage = failureMessage;
    }
}