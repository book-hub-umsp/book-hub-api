using BookHub.API.Contracts.REST.Requests.Account;
using BookHub.API.Models.Account;
using BookHub.API.Models.DomainEvents;
using BookHub.API.Models.DomainEvents.Account;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Abstractions.Logic.Converters.Account;

/// <summary>
/// Описывает конвертер запросов, относящихся к пользователям.
/// </summary>
public interface IUserRequestConverter
{
    public UserUpdatedBase Convert(Id<User> userId, UpdateUserProfileInfoRequest request);
}