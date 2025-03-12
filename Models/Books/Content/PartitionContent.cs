namespace BookHub.API.Models.Books.Content;

/// <summary>
/// Модель контента раздела книги.
/// </summary>
public sealed record class PartitionContent
{
    public string Value { get; }

    public PartitionContent(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        ArgumentOutOfRangeException.ThrowIfLessThan(value.Length, MIN_SYMBOLS_NUMBER);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value.Length, MAX_SYMBOLS_NUMBER);

        Value = value;
    }

    private const int MIN_SYMBOLS_NUMBER = 0;
    private const int MAX_SYMBOLS_NUMBER = 40000;
}