namespace BookHub.Models;

/// <summary>
/// Идентификатор сущности.
/// </summary>
/// <typeparam name="TEntity">
/// Тип сущности.
/// </typeparam>
/// <param name="Value">
/// Значение.
/// </param>
public sealed class Id<TEntity>(long Value);
