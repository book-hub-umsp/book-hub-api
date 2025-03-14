using BookHub.API.Models.Books.Content;
using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Models.DomainEvents.Books;

/// <summary>
/// Обновление по главе.
/// </summary>
/// <typeparam name="TAttribute">
/// Тип атрибута, который необходимо обновить.
/// </typeparam>
public sealed class PartitionUpdated<TAttribute> : UpdatedBase
{
    public override CompositeId<Partition, Id<Book>, PartitionSequenceNumber> EntityId => 
        (CompositeId<Partition, Id<Book>, PartitionSequenceNumber>)base.EntityId;

    public TAttribute Attribute { get; }

    public PartitionUpdated(
        CompositeId<Partition, Id<Book>, PartitionSequenceNumber> id,
        TAttribute attribute) 
        : base(id)
    {
        ArgumentNullException.ThrowIfNull(attribute);
        Attribute = attribute;
    }
}