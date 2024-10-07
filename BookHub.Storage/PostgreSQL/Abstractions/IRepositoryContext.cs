namespace BookHub.Storage.PostgreSQL.Abstractions;

public interface IRepositoryContext
{
    void SaveChanges();
}
