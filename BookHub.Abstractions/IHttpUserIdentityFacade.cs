using BookHub.Models;
using BookHub.Models.Account;

namespace BookHub.Abstractions;

/// <summary>
/// Фасад для получения информации о пользователе, в рамках HTTP запроса.
/// </summary>
public interface IHttpUserIdentityFacade
{
    public Id<User>? Id { get; }
}
