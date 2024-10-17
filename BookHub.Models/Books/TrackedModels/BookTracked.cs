using System.ComponentModel;
using System;
using BookHub.Models.Dispatching;

namespace BookHub.Models.Books.TrackedModels;

/// <summary>
/// Модель книги c возможностью отслеживания изменений.
/// </summary>
public sealed class BookTracked : Book
{
    public BookTracked(
        BookAuthor author,
        BookDefinitionTracked definition,
        BookText text,
        IEventDispatcher eventDispatcher,
        BookStatus status = BookStatus.Published)
        : base(author, definition, text, status)
    {
        _dispatcher = eventDispatcher 
            ?? throw new ArgumentNullException(nameof(eventDispatcher));
    }

    public void ChangeDefinition(BookDefinitionTracked newDefinition)
    {
        ArgumentNullException.ThrowIfNull(newDefinition);

        Definition = newDefinition;
    }

    public void ChangeText(BookText newText)
    {
        ArgumentNullException.ThrowIfNull(newText);

        Text = newText;
    }

    public void ChangeStatus(BookStatus newStatus)
    {
        if (!Enum.IsDefined(newStatus))
        {
            throw new InvalidEnumArgumentException(
                nameof(newStatus),
                (int)newStatus,
                typeof(BookStatus));
        }

        Status = newStatus;
    }

    private readonly IEventDispatcher _dispatcher;
}