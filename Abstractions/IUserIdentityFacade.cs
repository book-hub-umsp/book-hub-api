using BookHub.API.Models.Account;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Abstractions;

/// <summary>
/// Фасад для получения информации о пользователе, делающего запрос.
/// </summary>
public interface IUserIdentityFacade
{
    public Id<User>? Id { get; }
}
