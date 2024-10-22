using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BookHub.API.Authentification;

// Todo: add repo validation
public class CustomAuthorizationHandler : AuthorizationHandler<CustomRequirementMarker>
{
    public CustomAuthorizationHandler(ILogger<CustomAuthorizationHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CustomRequirementMarker requirement)
    {
        var email = context.User.FindFirst(JwtRegisteredClaimNames.Email);
        _logger.LogInformation($"Auth email is: {email}");

        await Task.Delay(100); // get REPOSITORY AND CHECK

        if (email is not null) // check repo email
        {
            context.Succeed(requirement);
        }
    }

    private readonly ILogger<CustomAuthorizationHandler> _logger;
}
