namespace BookHub.Storage.Models;

/// <summary>
/// Сам текст книги.
/// </summary>
public sealed record class BookText
{
    public string Content { get; }

    public BookText(string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(content);

        content = content.Trim();

        if (content.Length > MAX_LENGHT)
        {
            throw new ArgumentException($"Content max lenght must be {MAX_LENGHT} symbols.");
        }

        if (content.Length < MIN_LENGHT)
        {
            throw new ArgumentException($"Content min lenght must be {MAX_LENGHT} symbols.");
        }

        Content = content;
    }

    private const int MIN_LENGHT = 1;
    private const int MAX_LENGHT = 1_000_000;
}
