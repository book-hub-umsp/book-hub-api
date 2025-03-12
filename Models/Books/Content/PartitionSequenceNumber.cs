namespace BookHub.API.Models.Books.Content;

/// <summary>
/// Модель номера раздела.
/// </summary>
public sealed record class PartitionSequenceNumber
{
    public int Value { get; }

    public PartitionSequenceNumber(int value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, MIN_NUMBER);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, MAX_NUMBER);

        Value = value;
    }

    public static readonly int MIN_NUMBER = 1;
    public static readonly int MAX_NUMBER = 5;
}