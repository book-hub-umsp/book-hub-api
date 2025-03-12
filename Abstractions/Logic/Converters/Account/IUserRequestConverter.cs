using BookHub.API.Contracts.REST.Requests.Account;
using BookHub.API.Models;
using BookHub.API.Models.Account;
using BookHub.API.Models.DomainEvents;

namespace BookHub.API.Abstractions.Logic.Converters.Account;

/// <summary>
/// Описывает конвертер запросов, относящихся к пользователям.
/// </summary>
public interface IUserRequestConverter
{
    public UpdatedBase<User> Convert(Id<User> userId, UpdateUserProfileInfoRequest request);
}