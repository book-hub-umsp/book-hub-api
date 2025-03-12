using BookHub.API.Abstractions.Logic.Converters.Account;
using BookHub.API.Contracts.REST.Requests.Account;
using BookHub.API.Models;
using BookHub.API.Models.Account;
using BookHub.API.Models.DomainEvents;
using BookHub.API.Models.DomainEvents.Account;

namespace BookHub.API.Logic.Converters.Account;

/// <summary>
/// Конвертер запросов относящихся к пользователям.
/// </summary>
public sealed class UserRequestConverter : IUserRequestConverter
{
    public UpdatedBase<User> Convert(
        Id<User> userId,
        UpdateUserProfileInfoRequest request)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(request);

        return (request.NewName, request.About) switch
        {
            (not null, null) => new Updated<Name<User>>(userId, new(request.NewName)),

            (null, not null) => new Updated<About>(userId, new(request.About)),

            _ => throw new InvalidOperationException("Update parameters is invalid.")
        };
    }
}
