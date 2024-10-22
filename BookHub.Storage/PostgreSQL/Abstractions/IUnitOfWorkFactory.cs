namespace BookHub.Storage.PostgreSQL.Abstractions;

/// <summary>
/// Представляет фабрику для создания scoped единиц работы.
/// </summary>
public interface IUnitOfWorkFactory
{
    public IBooksUnitOfWork Create();
}