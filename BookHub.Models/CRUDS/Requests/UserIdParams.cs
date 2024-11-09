using BookHub.Models.Users;

namespace BookHub.Models.CRUDS.Requests;

/// <summary>
/// Параметры для запросов с уникальным ключом пользователя
/// </summary>
public sealed class UserIdParams
{
    public Id<User> Id { get; }

    public UserIdParams(Id<User> id)
    {
        Id = id;
    }
}
