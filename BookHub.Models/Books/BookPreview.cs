using BookHub.Models.Users;

namespace BookHub.Models.Books;

/// <summary>
/// Короткое описание книги.
/// </summary>
public sealed class BookPreview
{
    public Id<Book> Id { get; }

    public Name<Book> Title { get; }

    public BookGenre Genre { get; }

    public Id<User> AuthorId { get; }

    public BookPreview(
        Id<Book> id, 
        Name<Book> title, 
        BookGenre genre, 
        Id<User> authorId)
    {
        Id = id;
        Title = title;
        Genre = genre;
        AuthorId = authorId;
    }
}
