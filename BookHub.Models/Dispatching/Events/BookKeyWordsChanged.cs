using BookHub.Models.Books;
using System;

namespace BookHub.Models.Dispatching.Events;

/// <summary>
/// Событие, связанное с изменением ключевых слов в книге.
/// </summary>
public sealed class BookKeyWordsChanged : BookRelatedEvent
{
    public BookKeyWordsChanged(Id<Book> bookId, DateTimeOffset lastEditDate) : base(bookId, lastEditDate)
    {
    }
}