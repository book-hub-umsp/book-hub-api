using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Models.DomainEvents.Books;

public class BookUpdated<TAttribute> : BookUpdatedBase
{
    public TAttribute Attribute { get; }

    public BookUpdated(Id<Book> id, TAttribute attribute) : base(id)
    {
        ArgumentNullException.ThrowIfNull(attribute);
        Attribute = attribute;
    }
}
