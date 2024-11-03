using System.Security.Claims;

using BookHub.Logic.Services.Users;

using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BookHub.API.Authentification;

public class UserExistsAuthorizationHandler : AuthorizationHandler<UserExistsRequirementMarker>
{
    private readonly IUserService _userService;

    public UserExistsAuthorizationHandler(
        IUserService userService,
        ILogger<UserExistsAuthorizationHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(userService);
        _userService = userService;

        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        UserExistsRequirementMarker requirement)
    {
        var email = context.User.FindFirstValue(JwtRegisteredClaimNames.Email);

        if (email is null)
        {
            return;
        }

        var userInfo = await _userService.RegisterNewUserOrGetExistingAsync(
            new(new(email)),
            CancellationToken.None);

        _logger.LogInformation("Auth user name is: {UserName}", userInfo.Name);

        context.User.AddIdentity(new(
            [
                new Claim(
                    Auth.ClaimTypes.USER_ID_CLAIM_NAME, 
                    userInfo.Id.Value.ToString())
            ]));

        context.Succeed(requirement);
    }

    private readonly ILogger<UserExistsAuthorizationHandler> _logger;
}
