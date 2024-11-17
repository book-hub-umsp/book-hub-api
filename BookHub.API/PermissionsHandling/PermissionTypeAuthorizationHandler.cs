using System.Security.Claims;

using BookHub.Abstractions.Logic.Services.Account;
using BookHub.API.Authentification;

using Microsoft.AspNetCore.Authorization;

namespace BookHub.API.Roles;

/// <summary>
/// Промежуточный обработчик авторизации, проверяющий,
/// удовлетворяет ли необходимый permission для метода API опциям роли пользователя.
/// </summary>
public sealed class PermissionTypeAuthorizationHandler :
    AuthorizationHandler<PermissionTypeRequirement>
{
    public PermissionTypeAuthorizationHandler(
        IRolesService rolesService,
        ILogger<PermissionTypeAuthorizationHandler> logger)
    {
        _rolesService = rolesService
            ?? throw new ArgumentNullException(nameof(rolesService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionTypeRequirement requirement)
    {
        var userId = context.User.FindFirstValue(Auth.ClaimTypes.USER_ID_CLAIM_NAME);

        if (userId is null)
        {
            _logger.LogWarning("No authentificated user found in context");

            return;
        }

        var role = await _rolesService.GetUserRoleAsync(
            new(Convert.ToInt64(userId)),
            CancellationToken.None);

        if (role is null)
        {
            return;
        }

        if (role.Permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }

    private readonly IRolesService _rolesService;
    private readonly ILogger<PermissionTypeAuthorizationHandler> _logger;
}