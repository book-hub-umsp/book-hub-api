using BookHub.Contracts.REST.Requests.Users;
using BookHub.Models.DomainEvents;
using BookHub.Models.Users;
using BookHub.Models;

namespace BookHub.Abstractions.Logic.Converters;

/// <summary>
/// Описывает конвертер запросов, относящихся к пользователям.
/// </summary>
public interface IUserRequestConverter
{
    public UpdatedBase<User> Convert(Id<User> userId, UpdateUserProfileInfoRequest request);
}