namespace BookHub.Storage.Models;

public sealed record class BookBriefDescription
{
    public string Content { get; }

    public BookBriefDescription(string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(content);
        content = content.Trim();

        if (content.Length > MAX_LENGHT)
        {
            throw new ArgumentException($"Content max lenght must be {MAX_LENGHT} symbols");
        }

        if (content.Length < MIN_LENGHT)
        {
            throw new ArgumentException($"Content max lenght must be {MAX_LENGHT} symbols");
        }

        Content = content;
    }

    private const int MIN_LENGHT = 50;
    private const int MAX_LENGHT = 1000;
}