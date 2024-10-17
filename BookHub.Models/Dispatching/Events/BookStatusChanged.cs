using BookHub.Models.Books;
using System;

namespace BookHub.Models.Dispatching.Events;

/// <summary>
/// Событие, связанное с изменением статуса книги.
/// </summary>
public sealed class BookStatusChanged : BookRelatedEvent
{
    public BookStatus NewStatus { get; set; }

    public BookStatusChanged(
        Id<Book> bookId, 
        DateTimeOffset lastEditDate,
        BookStatus newStatus) 
        : base(bookId, lastEditDate)
    {
        NewStatus = newStatus;
    }
}