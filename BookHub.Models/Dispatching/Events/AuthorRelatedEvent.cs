using BookHub.Models.Books;
using System;

namespace BookHub.Models.Dispatching.Events;

public abstract class AuthorRelatedEvent : IDomainEvent
{
    public Id<BookAuthor> AuthorId { get; }

    public AuthorRelatedEvent(Id<BookAuthor> authorId)
    {
        AuthorId = authorId ?? throw new ArgumentNullException(nameof(authorId));
    }
}
