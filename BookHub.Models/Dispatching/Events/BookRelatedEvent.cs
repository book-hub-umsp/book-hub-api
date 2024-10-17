using BookHub.Models.Books;
using System;

namespace BookHub.Models.Dispatching.Events;

/// <summary>
/// Базовое событие, связанное с изменением книги.
/// </summary>
public abstract class BookRelatedEvent : IDomainEvent
{
    public Id<Book> BookId { get; }

    public DateTimeOffset LastEditDate { get; }

    protected BookRelatedEvent(
        Id<Book> bookId, 
        DateTimeOffset lastEditDate)
    {
        BookId = bookId ?? throw new ArgumentNullException(nameof(bookId));
        LastEditDate = lastEditDate;
    }
}
