using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BookHub.Models.Books;

/// <summary>
/// Описание книги.
/// </summary>
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
        if (!Enum.IsDefined(genre))
        {
            throw new InvalidEnumArgumentException(
                nameof(genre),
                (int)genre,
                typeof(BookGenre));
        }

        Genre =  genre;

        Name = name ?? throw new ArgumentNullException(nameof(name));
        BriefDescription = briefDescription 
            ?? throw new ArgumentNullException(nameof(keyWords));
        KeyWords = keyWords ?? throw new ArgumentNullException(nameof(keyWords));
    }
}