using System.Security.Claims;

using BookHub.Abstractions;
using BookHub.API.Authentification;
using BookHub.Models;
using BookHub.Models.Users;

namespace BookHub.API;

public class HttpUserIdentityFacade : IHttpUserIdentityFacade
{
    private readonly IHttpContextAccessor _contextAccessor;

    public Id<User>? Id { get; }

    public HttpUserIdentityFacade(IHttpContextAccessor contextAccessor)
    {
        ArgumentNullException.ThrowIfNull(contextAccessor);
        _contextAccessor = contextAccessor;

        Id = _contextAccessor.HttpContext?.User.FindFirstValue(Auth.ClaimTypes.USER_ID_CLAIM_NAME) is { } value
            ? new(long.Parse(value))
            : null;
    }
}
