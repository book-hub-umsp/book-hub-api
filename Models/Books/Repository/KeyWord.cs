namespace BookHub.API.Models.Books.Repository;

/// <summary>
/// Ключевое слово.
/// </summary>
public sealed record class KeyWord
{
    public Name<KeyWord> Content { get; }

    public KeyWord(Name<KeyWord> content)
    {
        ArgumentNullException.ThrowIfNull(content);

        var stringContent = content.Value.Trim();

        if (stringContent.Length > MAX_LENGHT)
        {
            throw new ArgumentException($"Content max lenght must be {MAX_LENGHT} symbols.");
        }

        if (stringContent.Length < MIN_LENGHT)
        {
            throw new ArgumentException($"Content min lenght must be {MAX_LENGHT} symbols.");
        }

        Content = new(stringContent);
    }

    private const int MIN_LENGHT = 1;
    private const int MAX_LENGHT = 20;
}