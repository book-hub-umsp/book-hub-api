namespace BookHub.API.Storage.PostgreSQL.Models;

/// <summary>
/// Модель раздела книги в хранилище.
/// </summary>
public sealed class Partition
{
    public long BookId { get; set; }

    public int SequenceNumber { get; set; }
    public string Content { get; set; } = null!;

    public Book Book { get; set; } = null!;
}