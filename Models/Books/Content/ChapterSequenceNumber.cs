namespace BookHub.API.Models.Books.Content;

/// <summary>
/// Модель номера главы.
/// </summary>
public sealed record class ChapterSequenceNumber
{
    public int Value { get; }

    public ChapterSequenceNumber(int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, MIN_NUMBER);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, MAX_NUMBER);

        Value = value;
    }

    public static readonly int MIN_NUMBER = 1;
    public static readonly int MAX_NUMBER = 5;
}