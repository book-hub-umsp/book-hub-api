namespace BookHub.Storage.Models;

/// <summary>
/// Модель книги.
/// </summary>
public sealed class Book
{
    public BookAuthor Author { get; }

    /// <remarks>
    /// Подразумевается неизменяемым.
    /// </remarks>
    public BookGenre Genre { get; }

    public Name<Book> Name { get; private set; }

    public BookStatus Status { get; private set; }

    public HashSet<KeyWord> KeyWords { get; private set; }

    public BookBriefDescription BriefDescription { get; private set; }

    public DateTimeOffset CreationDate { get; }

    public DateTimeOffset LastEditDate { get; private set; }

    public

    public Book()
    {
        CreationDate = DateTimeOffset.UtcNow;
    }
}
