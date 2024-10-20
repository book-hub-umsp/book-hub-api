using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BookHub.Models.Books;

/// <summary>
/// Описание книги.
/// </summary>
public sealed class BookDescription
{
    /// <remarks>
    /// Подразумевается неизменяемым.
    /// </remarks>
    public BookGenre Genre { get; }

    public Name<Book> Title { get; private set; }

    public BookAnnotation BookAnnotation { get; private set; }

    public HashSet<KeyWord> KeyWords { get; private set; } = [];

    public BookDescription(
        BookGenre genre, 
        Name<Book> title, 
        BookAnnotation bookAnnotation)
    {
        if (!Enum.IsDefined(genre))
        {
            throw new InvalidEnumArgumentException(
                nameof(genre),
                (int)genre,
                typeof(BookGenre));
        }

        Genre =  genre;

        Title = title ?? throw new ArgumentNullException(nameof(title));
        BookAnnotation = bookAnnotation
            ?? throw new ArgumentNullException(nameof(bookAnnotation));
    }

    public void ChangeTitle(Name<Book> newTitle)
    {
        ArgumentNullException.ThrowIfNull(newTitle);

        Title = newTitle;
    }

    public void ChangeBriefDescription(BookAnnotation newBookAnnotation)
    {
        ArgumentNullException.ThrowIfNull(newBookAnnotation);

        BookAnnotation = newBookAnnotation;
    }

    public void AddKeyWords(IReadOnlySet<KeyWord> keyWords)
    {
        ArgumentNullException.ThrowIfNull(keyWords);

        foreach (var keyWord in keyWords)
        {
            _ = KeyWords.Add(keyWord);
        }
    }
}