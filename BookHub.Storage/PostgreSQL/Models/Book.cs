using BookHub.Models;
using BookHub.Models.Books;

namespace BookHub.Storage.PostgreSQL.Models;

/// <summary>
/// Книга.
/// </summary>
public sealed class Book
{
    public Id<Book> Id { get; set; }

    public required Name<Book> Caption { get; set; }

    public required Id<Author> AuthorId { get; set; }

    public required BookGenre BookGenre { get; set; }

    public required string BookBriefDescription { get; set; }

    public required string BookText { get; set; }

    public required BookStatus BookStatus { get; set; }

    public required DateTimeOffset CreationDate { get; set; }

    public required DateTimeOffset LastEditDate { get; set; }

    public HashSet<Author> LikedUsers { get; set; } = null!;

    public HashSet<KeyWord> Keywords { get; set; } = null!;
}