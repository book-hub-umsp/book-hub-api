using BookHub.Abstractions.Logic.Services.Account;
using BookHub.Contracts;
using BookHub.Contracts.REST.Responses.Account.Permissions;
using BookHub.Models.Account;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookHub.API.Controllers;

/// <summary>
/// Контроллер для управления permissions.
/// </summary>
[Authorize]
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public sealed class PermissionsController : ControllerBase
{
    public PermissionsController(
        IRolesService rolesService,
        ILogger<PermissionsController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _rolesService = rolesService
            ?? throw new ArgumentNullException(nameof(rolesService));
    }

    /// <summary>
    /// Получает все permissions в системе.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<PermissionsListResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Route("all")]
    public IActionResult GetAllPermissions()
    {
        _logger.LogInformation("Getting all system permissions");

        var permissions = Enum.GetValues<PermissionType>();

        return Ok(new PermissionsListResponse
        {
            Permissions = permissions
        });
    }

    /// <summary>
    /// Получает все permissions в системе.
    /// </summary>
    /// <param name="userId">
    /// Идентификатор пользователя.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    [HttpGet]
    [ProducesResponseType<PermissionsListResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Route("user/{userId}")]
    public async Task<IActionResult> GetUserPermissionsAsync(
        [FromRoute] long userId,
        CancellationToken token)
    {
        _logger.LogInformation("Getting permissions for user {UserId}", userId);

        var userRole = await _rolesService.GetUserRoleAsync(new(userId), token);

        if (userRole is null)
        {
            return BadRequest(new FailureCommandResultResponse
            {
                FailureMessage = $"User with id {userId} not found"
            });
        }

        _logger.LogInformation(
            "{PermissionsCount} permissions were found for user {UserId}",
            userRole.Permissions.Count,
            userId);

        return Ok(new PermissionsListResponse
        {
            Permissions = userRole.Permissions
        });
    }

    private readonly IRolesService _rolesService;
    private readonly ILogger<PermissionsController> _logger;
}