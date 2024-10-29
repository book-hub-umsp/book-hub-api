namespace BookHub.Models.CRUDS.Responces;

/// <summary>
/// Класс, отвечающий за результат исполнения команды c контентом.
/// </summary>
public sealed class CommandResultWithContent : CommandExecutionResult
{
    public object? Content { get; }

    public CommandResultWithContent(
        CommandResult commandResult, 
        string? failureMessage,
        object? content = null) 
        : base(commandResult, failureMessage)
    {
        Content = content;
    }
}