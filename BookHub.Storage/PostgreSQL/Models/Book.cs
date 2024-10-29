using BookHub.Models.Books;

namespace BookHub.Storage.PostgreSQL.Models;

/// <summary>
/// Книга.
/// </summary>
public sealed class Book
{
    public long Id { get; set; }

    public required string Title { get; set; }

    public required long AuthorId { get; set; }

    public required long BookGenreId { get; set; }

    public BookGenre BookGenre { get; set; } = null!;

    public required string BookAnnotation { get; set; }

    public required BookStatus BookStatus { get; set; }

    public string? KeyWordsContent { get; set; } = null!;

    public required DateTimeOffset CreationDate { get; set; }

    public required DateTimeOffset LastEditDate { get; set; }
}