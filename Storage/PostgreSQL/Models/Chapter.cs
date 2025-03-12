namespace BookHub.API.Storage.PostgreSQL.Models;

/// <summary>
/// Модель главы в хранилище.
/// </summary>
public sealed class Chapter : IKeyable
{
    public long Id { get; set; }

    public int SequenceNumber { get; set; }

    public long BookId { get; set; }

    public Book Book { get; set; } = null!;

    public string Content { get; set; } = null!;
}