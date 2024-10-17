using BookHub.Models.Books;
using System;

namespace BookHub.Models.Dispatching.Events;

/// <summary>
/// Событие, связанное с изменением названия книги.
/// </summary>
public sealed class BookCaptionChanged : BookRelatedEvent
{
    public BookCaptionChanged(Id<Book> bookId, DateTimeOffset lastEditDate) : base(bookId, lastEditDate)
    {
    }
}
