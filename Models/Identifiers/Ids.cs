namespace BookHub.API.Models.Identifiers;

/// <summary>
/// Идентификатор сущности.
/// </summary>
/// <typeparam name="TEntity">
/// Тип сущности.
/// </typeparam>
/// <param name="Value">
/// Значение.
/// </param>
public sealed record class Id<TEntity>(long Value) : IIdentifier;

public sealed record class Id<TEntity, TKey>(TKey Value) : IIdentifier;

public sealed record class CompositeId<TEntity, TKey1, TKey2> : IIdentifier
{
    public TKey1 Key1 { get; }

    public TKey2 Key2 { get; }

    public CompositeId(TKey1 key1, TKey2 key2)
    {
        ArgumentNullException.ThrowIfNull(key1);
        Key1 = key1;

        ArgumentNullException.ThrowIfNull(key2);
        Key2 = key2;
    }

    public void Deconstruct(out TKey1 key1, out TKey2 key2)
    {
        key1 = Key1; 
        key2 = Key2;
    }
}
