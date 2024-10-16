using System.Collections.Immutable;

namespace BookHub.Storage.Models;

public sealed class BookDefinition
{
    /// <remarks>
    /// Подразумевается неизменяемым.
    /// </remarks>
    public BookGenre Genre { get; }

    public Name<Book> Name { get; private set; }

    public BookBriefDescription BriefDescription { get; private set; }

    public HashSet<KeyWord>? KeyWords { get; private set; }

    public BookDefinition(
        BookGenre genre, 
        Name<Book> name, 
        BookBriefDescription briefDescription,
        HashSet<KeyWord> keyWords = null!)
    {
        Genre = genre;
        Name = name;
        KeyWords = keyWords;
        BriefDescription = briefDescription;
    }
}
