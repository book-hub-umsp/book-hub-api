using BookHub.Models.Books;

namespace BookHub.Models.Dispatching.Events;

/// <summary>
/// Событие, связанное с изменением избранного автора.
/// </summary>
public sealed class AuthorFavouriteBooksChanged : AuthorRelatedEvent
{
    public AuthorFavouriteBooksChanged(Id<BookAuthor> authorId) : base(authorId)
    {
    }
}
