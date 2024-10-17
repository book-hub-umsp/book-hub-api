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

    public Name<Book> Caption { get; private set; }

    public BookBriefDescription BriefDescription { get; private set; }

    public HashSet<KeyWord> KeyWords { get; private set; } = [];

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

    public void ChangeCaption(Name<Book> newCaption)
    {
        ArgumentNullException.ThrowIfNull(nameof(newCaption));

        Caption = newCaption;
    }

    public void ChangeBriefDescription(BookBriefDescription newBriefDescription)
    {
        ArgumentNullException.ThrowIfNull(nameof(newBriefDescription));

        BriefDescription = newBriefDescription;
    }

    public void AddKeyWords(IReadOnlySet<KeyWord> keyWords)
    {
        ArgumentNullException.ThrowIfNull(nameof(keyWords));

        foreach (var keyWord in keyWords)
        {
            _ = KeyWords.Add(keyWord);
        }
    }
}