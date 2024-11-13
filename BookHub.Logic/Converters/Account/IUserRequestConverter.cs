using BookHub.Contracts.REST.Requests.Account;
using BookHub.Models;
using BookHub.Models.Account;
using BookHub.Models.DomainEvents;

namespace BookHub.Logic.Converters.Account;

/// <summary>
/// Описывает конвертер запросов, относящихся к пользователям.
/// </summary>
public interface IUserRequestConverter
{
    public UpdatedBase<User> Convert(Id<User> userId, UpdateUserProfileInfoRequest request);
}