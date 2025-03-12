namespace BookHub.API.Models.DomainEvents;

/// <summary>
/// Базовая модель обновления.
/// </summary>
/// <typeparam name="TEntity">
/// Конкретный тип обновляемой сущности.
/// </typeparam>
public abstract class UpdatedBase<TEntity>
{
    public Id<TEntity> Id { get; }

    protected UpdatedBase(Id<TEntity> id)
    {
        ArgumentNullException.ThrowIfNull(id);
        Id = id;
    }
}
