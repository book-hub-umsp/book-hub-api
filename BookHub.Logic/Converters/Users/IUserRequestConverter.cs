using BookHub.Contracts.REST.Requests.Users;
using BookHub.Models;
using BookHub.Models.DomainEvents;
using BookHub.Models.Users;

namespace BookHub.Logic.Converters.Users;

/// <summary>
/// Описывает конвертер запросов, относящихся к пользователям.
/// </summary>
public interface IUserRequestConverter
{
    public UpdatedBase<User> Convert(Id<User> userId, UpdateUserProfileInfoRequest request);
}