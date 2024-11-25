using BookHub.Models.Account;
using BookHub.Models.Books.Content;

using System;

namespace BookHub.Models.DomainEvents.Books;

/// <summary>
/// Обновление по главе.
/// </summary>
/// <typeparam name="TAttribute">
/// Тип атрибута, который необходимо обновить.
/// </typeparam>
public sealed class UpdatedChapter<TAttribute> : UpdatedBase<Chapter>
{
    public TAttribute Attribute { get; }

    public UpdatedChapter(
        Id<Chapter> id, 
        TAttribute attribute) : base(id)
    {
        ArgumentNullException.ThrowIfNull(attribute);
        Attribute = attribute;
    }
}