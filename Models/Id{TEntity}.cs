namespace BookHub.API.Models;

/// <summary>
/// Идентификатор сущности.
/// </summary>
/// <typeparam name="TEntity">
/// Тип сущности.
/// </typeparam>
/// <param name="Value">
/// Значение.
/// </param>
public sealed record class Id<TEntity>(long Value);
