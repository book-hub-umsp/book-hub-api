using System.Collections.Generic;
using System.ComponentModel;
using System;
using BookHub.Models.Dispatching;

namespace BookHub.Models.Books.TrackedModels;

/// <summary>
/// Модель описания книги с возможностью отслеживания изменений.
/// </summary>
public sealed class BookDefinitionTracked : BookDefinition
{
    public BookDefinitionTracked(
        BookGenre genre,
        Name<Book> caption,
        BookBriefDescription briefDescription,
        IEventDispatcher eventDispatcher)
        : base(genre, caption, briefDescription)
    {
        _dispatcher = eventDispatcher 
            ?? throw new ArgumentNullException(nameof(eventDispatcher));
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

    private readonly IEventDispatcher _dispatcher;
}
