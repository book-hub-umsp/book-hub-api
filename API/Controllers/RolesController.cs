using System.ComponentModel.DataAnnotations;

using BookHub.API.Abstractions.Logic.Services.Account;
using BookHub.API.Contracts;
using BookHub.API.Contracts.REST.Responses.Account.Roles;
using BookHub.API.Models.Account;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookHub.API.Service.Controllers;

/// <summary>
/// Контроллер для управления permissions.
/// </summary>
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[Authorize(Policy = nameof(Permission.ModerateAccounts))]
public sealed class RolesController : ControllerBase
{
    public RolesController(
        IRolesService rolesService,
        ILogger<RolesController> logger)
    {
        _rolesService = rolesService
            ?? throw new ArgumentNullException(nameof(rolesService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Получает список ролей в системе.
    /// </summary>
    [HttpGet]
    [ProducesResponseType<RolesListResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Route("all")]
    public async Task<ActionResult<RolesListResponse>> GetAllRolesAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var rolesList = await _rolesService.GetAllRolesAsync(token);

        var dto = new RolesListResponse
        {
            Roles = rolesList
                .Select(RoleDTO.FromDomain)
                .ToList()
        };

        return Ok(dto);
    }

    /// <summary>
    /// Изменяет permissions для существующей в системе роли.
    /// </summary>
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("{roleId}/permissions/change")]
    public async Task<IActionResult> ChangeRolePermissionsAsync(
        [FromRoute] long roleId,
        [Required][FromQuery] IReadOnlyCollection<Permission> permissions,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        try
        {
            await _rolesService.ChangeRolePermissionsAsync(
                new(roleId),
                permissions.ToHashSet(),
                token);

            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    /// <summary>
    /// Изменяет роль для указанного пользователя.
    /// </summary>
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("users/{userId}/set/{roleId}")]
    public async Task<IActionResult> ChangeUserRoleAsync(
        [FromRoute] long userId,
        [FromRoute] long roleId,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        try
        {
            await _rolesService.ChangeUserRoleAsync(
                new(userId),
                new(roleId),
                token);

            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    private readonly IRolesService _rolesService;
    private readonly ILogger<RolesController> _logger;
}