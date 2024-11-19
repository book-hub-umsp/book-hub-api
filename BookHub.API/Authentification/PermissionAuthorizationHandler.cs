using BookHub.Abstractions;
using BookHub.Abstractions.Logic.Services.Account;

using Microsoft.AspNetCore.Authorization;

namespace BookHub.API.Authentification;

/// <summary>
/// Промежуточный обработчик авторизации, проверяющий,
/// удовлетворяет ли необходимый permission для метода API опциям роли пользователя.
/// </summary>
public sealed class PermissionAuthorizationHandler :
    AuthorizationHandler<PermissionRequirement>
{
    public PermissionAuthorizationHandler(
        IHttpUserIdentityFacade idFacade,
        IRolesService rolesService,
        ILogger<PermissionAuthorizationHandler> logger)
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
        PermissionRequirement requirement)
    {
        var userId = _idFacade.Id;

        if (userId is null)
        {
            _logger.LogWarning("No authentificated user found in context");

            return;
        }

        var role = await _rolesService.GetUserRoleAsync(
            userId,
            CancellationToken.None);

        if (role.Permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }

    private readonly IHttpUserIdentityFacade _idFacade;
    private readonly IRolesService _rolesService;
    private readonly ILogger<PermissionAuthorizationHandler> _logger;
}