using System.Security.Claims;

using BookHub.Logic.Services.Account;

using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BookHub.API.Roles;

/// <summary>
/// Промежуточный обработчик авторизации, проверяющий,
/// удовлетворяет ли необходимый клэйм для метода API опциям роли пользователя.
/// </summary>
public sealed class ClaimTypeAuthorizationHandler : 
    AuthorizationHandler<ClaimTypeRequirement>
{
    public ClaimTypeAuthorizationHandler(
        IUserService userService,
        ILogger<ClaimTypeAuthorizationHandler> logger)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        ClaimTypeRequirement requirement)
    {
        var email = context.User.FindFirstValue(JwtRegisteredClaimNames.Email);

        if (email is null)
        {
            return;
        }

        var role = await _userService.GetUserRoleAsync(new(email), CancellationToken.None);

        if (role is null)
        {
            return;
        }

        if (role.Claims.Contains(requirement.ClaimType))
        {
            context.Succeed(requirement);
        }
    }

    private readonly IUserService _userService;
    private readonly ILogger<ClaimTypeAuthorizationHandler> _logger;
}