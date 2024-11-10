using BookHub.Models;
using BookHub.Models.Users;

namespace BookHub.Abstractions;

/// <summary>
/// Фасад для получения информации о пользователе, в рамках HTTP запроса.
/// </summary>
public interface IHttpUserIdentityFacade
{
    public Id<User>? Id { get; }
}
