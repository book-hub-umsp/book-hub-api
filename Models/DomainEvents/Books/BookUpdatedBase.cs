using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Models.DomainEvents.Books;

public abstract class BookUpdatedBase : UpdatedBase
{
    public override Id<Book> EntityId => (Id<Book>)base.EntityId;

    protected BookUpdatedBase(Id<Book> id) : base(id)
    {
    }
}
