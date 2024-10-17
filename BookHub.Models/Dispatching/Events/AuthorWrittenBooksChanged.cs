using BookHub.Models.Books;

namespace BookHub.Models.Dispatching.Events;

/// <summary>
/// Событие, связанное с изменением коллекции
/// написанных автором книг.
/// </summary>
public sealed class AuthorWrittenBooksChanged : AuthorRelatedEvent
{
    public AuthorWrittenBooksChanged(Id<BookAuthor> authorId) : base(authorId)
    {
    }
}
