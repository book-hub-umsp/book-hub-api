﻿using System.Security.Claims;

using BookHub.Abstractions;
using BookHub.API.Authentification;
using BookHub.Models;
using BookHub.Models.Account;

namespace BookHub.API;

/// <summary>
/// Фасад для получения информации об авторизованном пользователе.
/// </summary>
public sealed class HttpUserIdentityFacade : IUserIdentityFacade
{
    private readonly IHttpContextAccessor _contextAccessor;

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    /// <remarks>
    /// Вне контекста авторизации имеет значение <see langword="null"/>.
    /// </remarks>
    public Id<User>? Id =>
        _contextAccessor.HttpContext?.User.FindFirstValue(Auth.ClaimTypes.USER_ID_CLAIM_NAME) is { } value
            ? new(long.Parse(value))
            : null;

    public HttpUserIdentityFacade(IHttpContextAccessor contextAccessor)
    {
        ArgumentNullException.ThrowIfNull(contextAccessor);
        _contextAccessor = contextAccessor;
    }
}
