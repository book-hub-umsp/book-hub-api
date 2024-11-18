using System.Security.Claims;

using BookHub.Abstractions;
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
        IHttpUserIdentityFacade idFacade,
        IRolesService rolesService,
        ILogger<PermissionTypeAuthorizationHandler> logger)
    {
        _idFacade = idFacade
            ?? throw new ArgumentNullException(nameof(idFacade));
        _rolesService = rolesService
            ?? throw new ArgumentNullException(nameof(rolesService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionTypeRequirement requirement)
    {
        var userId = _idFacade.Id;

        if (userId is null)
        {
            _logger.LogWarning("No authentificated user found in context");

            return;
        }

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

    private readonly IHttpUserIdentityFacade _idFacade;
    private readonly IRolesService _rolesService;
    private readonly ILogger<PermissionTypeAuthorizationHandler> _logger;
}