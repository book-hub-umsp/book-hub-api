using System.Security.Claims;

using BookHub.Abstractions.Logic.Services.Account;
using BookHub.API.Authentification;

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
        IRolesService rolesService,
        ILogger<ClaimTypeAuthorizationHandler> logger)
    {
        _rolesService = rolesService 
            ?? throw new ArgumentNullException(nameof(rolesService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        ClaimTypeRequirement requirement)
    {
        var userId = context.User.FindFirstValue(Auth.ClaimTypes.USER_ID_CLAIM_NAME);

        if (userId is null)
        {
            _logger.LogWarning("No authentificated user found");

            return;
        }

        var role = await _rolesService.GetUserRoleAsync(new(Convert.ToInt64(userId)), CancellationToken.None);

        if (role is null)
        {
            return;
        }

        if (role.Claims.Contains(requirement.Claim))
        {
            context.Succeed(requirement);
        }
    }

    private readonly IRolesService _rolesService;
    private readonly ILogger<ClaimTypeAuthorizationHandler> _logger;
}