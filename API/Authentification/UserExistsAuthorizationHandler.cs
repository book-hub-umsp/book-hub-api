using System.Net.Mail;
using System.Security.Claims;

using BookHub.API.Abstractions.Logic.Services.Account;

using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BookHub.API.Service.Authentification;

/// <summary>
/// Промежуточный обработчик авторизации, проверяющий наличие пользовательского профиля на портале.
/// </summary>
public class UserExistsAuthorizationHandler : AuthorizationHandler<UserExistsRequirementMarker>
{
    private readonly IUserService _userService;
    private readonly ILogger<UserExistsAuthorizationHandler> _logger;

    public UserExistsAuthorizationHandler(
        IUserService userService,
        ILogger<UserExistsAuthorizationHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(userService);
        _userService = userService;

        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    /// <inheritdoc/>
    override protected async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        UserExistsRequirementMarker requirement)
    {
        var email = context.User.FindFirstValue(JwtRegisteredClaimNames.Email);

        if (email is null)
        {
            return;
        }

        var mailAddress = new MailAddress(email);
        var userInfo = await _userService.FindUserProfileInfoByEmailAsync(mailAddress, CancellationToken.None);

        if (userInfo is null)
        {
            if (!requirement.NeedRegisterIfNotExists)
            {
                return;
            }
            
            userInfo = await _userService.RegisterNewUserAsync(new(mailAddress), CancellationToken.None);
        }

        _logger.LogInformation("Auth user name is: {UserName}", userInfo.Name);

        context.User.AddIdentity(new(
            [
                new Claim(
                    Auth.ClaimTypes.USER_ID_CLAIM_NAME,
                    userInfo.Id.Value.ToString())
            ]));

        context.Succeed(requirement);
    }
}