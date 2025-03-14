using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Models.Books;

/// <summary>
/// Параметры команды добавления новой книги.
/// </summary>
public sealed class CreatingBook
{
    public Id<BookGenre> GenreId { get; }

    public Name<Book> Title { get; }

    public BookAnnotation Annotation { get; }

    public IReadOnlySet<Id<KeyWord>> Keywords { get; }

    public CreatingBook(
        Id<BookGenre> genre,
        Name<Book> title,
        BookAnnotation annotation)
    {
        GenreId = genre ?? throw new ArgumentNullException(nameof(genre));
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Annotation = annotation ?? throw new ArgumentNullException(nameof(annotation));
        Keywords = new HashSet<Id<KeyWord>>();
    }

    public CreatingBook(
        Id<BookGenre> genre,
        Name<Book> title,
        BookAnnotation annotation,
        IReadOnlySet<Id<KeyWord>> keyWords)
        : this(genre, title, annotation)
    {
        ArgumentNullException.ThrowIfNull(keyWords);

        Keywords = keyWords.ToHashSet();
    }
}