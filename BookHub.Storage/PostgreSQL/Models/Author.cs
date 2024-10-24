namespace BookHub.Storage.PostgreSQL.Models;

/// <summary>
/// Автор книги.
/// </summary>
public sealed class Author
{
    public long Id { get; set; }

    public required string Name { get; set; }

    public ICollection<Book> WrittenBooks { get; set; } = null!;
}