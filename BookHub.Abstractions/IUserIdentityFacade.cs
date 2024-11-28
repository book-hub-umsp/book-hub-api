using BookHub.Models;
using BookHub.Models.Account;

namespace BookHub.Abstractions;

/// <summary>
/// Фасад для получения информации о пользователе, делающего запрос.
/// </summary>
public interface IUserIdentityFacade
{
    public Id<User>? Id { get; }
}
