using BookHub.Models;

namespace BookHub.Storage.PostgreSQL.Models;

/// <summary>
/// Автор книги.
/// </summary>
public sealed class Author
{
    public Id<Author> Id { get; set; } 

    public required Name<Author> Name { get; set; }

    public HashSet<Book> WrittenBooks { get; set; } = null!;

    public HashSet<Book> FavouriteBooks { get; set; } = null!;
}
