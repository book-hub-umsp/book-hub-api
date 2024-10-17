using BookHub.Models.Books;
using System;

namespace BookHub.Models.Dispatching.Events;

/// <summary>
/// Событие, связанное с изменением статуса книги.
/// </summary>
public sealed class BookTextChanged : BookRelatedEvent
{
    public BookText NewText { get; set; }

    public BookTextChanged(
        Id<Book> bookId,
        DateTimeOffset lastEditDate,
        BookText bookText)
        : base(bookId, lastEditDate)
    {
        NewText = bookText;
    }
}