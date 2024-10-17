using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BookHub.Models.Books;

/// <summary>
/// Описание книги.
/// </summary>
public class BookDefinition
{
    /// <remarks>
    /// Подразумевается неизменяемым.
    /// </remarks>
    public BookGenre Genre { get; }

    public Name<Book> Caption { get; protected set; }

    public BookBriefDescription BriefDescription { get; protected set; }

    public HashSet<KeyWord> KeyWords { get; protected set; } = [];

    public BookDefinition(
        BookGenre genre, 
        Name<Book> caption, 
        BookBriefDescription briefDescription)
    {
        if (!Enum.IsDefined(genre))
        {
            throw new InvalidEnumArgumentException(
                nameof(genre),
                (int)genre,
                typeof(BookGenre));
        }

        Genre =  genre;

        Caption = caption ?? throw new ArgumentNullException(nameof(caption));
        BriefDescription = briefDescription 
            ?? throw new ArgumentNullException(nameof(briefDescription));
    }
}