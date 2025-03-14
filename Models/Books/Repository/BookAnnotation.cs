namespace BookHub.API.Models.Books.Repository;

/// <summary>
/// Аннотация книги.
/// </summary>
public sealed record class BookAnnotation
{
    public string Content { get; }

    public BookAnnotation(string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(content);
        content = content.Trim();

        ArgumentOutOfRangeException.ThrowIfGreaterThan(content.Length, MAX_LENGHT);
        ArgumentOutOfRangeException.ThrowIfLessThan(content.Length, MIN_LENGHT);

        Content = content;
    }

    private const int MIN_LENGHT = 50;
    private const int MAX_LENGHT = 1000;
}