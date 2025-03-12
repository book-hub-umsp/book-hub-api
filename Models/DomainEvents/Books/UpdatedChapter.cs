using BookHub.API.Models.Books.Content;
using BookHub.API.Models.Books.Repository;

namespace BookHub.API.Models.DomainEvents.Books;

/// <summary>
/// Обновление по главе.
/// </summary>
/// <typeparam name="TAttribute">
/// Тип атрибута, который необходимо обновить.
/// </typeparam>
public sealed class UpdatedChapter<TAttribute> : UpdatedBase<Chapter>
{
    public Id<Book> BookId { get; }

    public TAttribute Attribute { get; }

    public UpdatedChapter(
        Id<Chapter> id,
        Id<Book> bookId,
        TAttribute attribute) : base(id)
    {
        BookId = bookId ?? throw new ArgumentNullException(nameof(bookId));
        Attribute = attribute ?? throw new ArgumentException(nameof(attribute));
    }
}