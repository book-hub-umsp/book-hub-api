namespace BookHub.API.Storage.PostgreSQL.Models;

/// <summary>
/// Модель тэга в хранилище.
/// </summary>
public sealed class Keyword : IKeyable
{
    public long Id { get; set; }

    public string Value { get; set; } = null!;

    public ICollection<KeywordLink> BooksLinks { get; set; } = null!;
}