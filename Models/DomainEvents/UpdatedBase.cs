using BookHub.API.Models.Identifiers;

namespace BookHub.API.Models.DomainEvents;

/// <summary>
/// Базовая модель обновления.
/// </summary>
public abstract class UpdatedBase
{
    public virtual IIdentifier EntityId { get; }

    protected UpdatedBase(IIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);
        EntityId = id;
    }
}
