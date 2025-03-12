using System.Security.Claims;

using BookHub.API.Abstractions;
using BookHub.API.Models;
using BookHub.API.Models.Account;
using BookHub.API.Service.Authentification;

namespace BookHub.API.Service;

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
