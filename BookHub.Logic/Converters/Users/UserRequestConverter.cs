﻿using BookHub.Contracts.REST.Requests.Users;
using BookHub.Models;
using BookHub.Models.DomainEvents;
using BookHub.Models.DomainEvents.Users;
using BookHub.Models.Users;

namespace BookHub.Logic.Converters.Users;

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
