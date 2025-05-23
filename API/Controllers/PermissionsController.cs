﻿using BookHub.API.Abstractions.Logic.Services.Account;
using BookHub.API.Contracts;
using BookHub.API.Contracts.REST.Responses.Account.Permissions;
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
    public ActionResult<PermissionsListResponse> GetAllPermissions()
    {
        var permissions = Enum.GetValues<Permission>();

        return Ok(new PermissionsListResponse
        {
            Permissions = permissions.ToList(),
        });
    }

    /// <summary>
    /// Получает все permissions у пользователя.
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
        token.ThrowIfCancellationRequested();

        try
        {
            var userRole = await _rolesService.GetUserRoleAsync(new(userId), token);

            _logger.LogDebug(
                "{PermissionsCount} permissions were found for user {UserId}",
                userRole.Permissions.Count,
                userId);

            return Ok(new PermissionsListResponse
            {
                Permissions = userRole.Permissions
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    private readonly IRolesService _rolesService;
    private readonly ILogger<PermissionsController> _logger;
}