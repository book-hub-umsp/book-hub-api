using BookHub.Models.Books;

namespace BookHub.Storage.PostgreSQL.Models;

/// <summary>
/// Жанр книги.
/// </summary>
public sealed class BookGenre
{
    public long Id { get; set; }

    public Genre Genre { get; set; }

    public ICollection<Book> Books { get; set; } = null!;
}