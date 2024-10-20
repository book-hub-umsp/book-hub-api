using System;

namespace BookHub.Models.Books;

/// <summary>
/// Ключевое слово.
/// </summary>
public sealed class KeyWord
{
    public Id<KeyWord> Id { get; }

    public Name<KeyWord> Content { get; }

    public KeyWord(Id<KeyWord> id, Name<KeyWord> content)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));

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