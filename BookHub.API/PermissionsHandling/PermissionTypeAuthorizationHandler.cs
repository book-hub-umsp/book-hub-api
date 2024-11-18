using System.Security.Claims;

using BookHub.Abstractions.Logic.Services.Account;
using BookHub.API.Authentification;
using BookHub.Models;
using BookHub.Models.Account;

using Microsoft.AspNetCore.Authorization;

using Newtonsoft.Json.Linq;

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
        var userStringId = context.User.FindFirstValue(Auth.ClaimTypes.USER_ID_CLAIM_NAME);

        if (userStringId is null)
        {
            _logger.LogWarning("No authentificated user found in context");

            return;
        }

        var userId = new Id<User>(Convert.ToInt64(userStringId));

        try
        {
            var role = await _rolesService.GetUserRoleAsync(
                userId,
                CancellationToken.None);

            if (role.Permissions.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }
        catch (InvalidOperationException)
        {
            _logger.LogWarning("Not found user with user id {UserId}", userId.Value);

            return;
        }
    }

    private readonly IRolesService _rolesService;
    private readonly ILogger<PermissionTypeAuthorizationHandler> _logger;
}