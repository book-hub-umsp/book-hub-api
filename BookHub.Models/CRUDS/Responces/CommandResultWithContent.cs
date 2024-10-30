namespace BookHub.Models.CRUDS.Responces;

/// <summary>
/// Класс, отвечающий за результат исполнения команды c контентом.
/// </summary>
public sealed class CommandResultWithContent : CommandExecutionResult
{
    public object? Content { get; }

    public CommandResultWithContent(
        CommandResult commandResult,
        object? content = null,
        string? failureMessage = null) 
        : base(commandResult, failureMessage)
    {
        Content = content;
    }
}