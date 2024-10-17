using BookHub.Models.Books;
using System;

namespace BookHub.Models.Dispatching.Events;

/// <summary>
/// Событие, связанное с изменением краткого описания книги.
/// </summary>
public sealed class BookBriefDescriptionChanged : BookRelatedEvent
{
    public BookBriefDescriptionChanged(Id<Book> bookId, DateTimeOffset lastEditDate) : base(bookId, lastEditDate)
    {
    }
}
