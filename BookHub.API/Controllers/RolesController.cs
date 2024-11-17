﻿using BookHub.Abstractions.Logic.Services.Account;
using BookHub.Contracts.REST.Requests.Account.Roles;
using BookHub.Contracts;
using BookHub.Models.Account;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Mvc;

namespace BookHub.API.Controllers;

/// <summary>
/// Контроллер для управления permissions.
/// </summary>
[Authorize]
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class RolesController : ControllerBase
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
    /// Добавляет новую роль в системе.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = nameof(ClaimType.AddNewRole))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("add")]
    public async Task<IActionResult> AddNewRoleAsync(
        [Required][NotNull] AddNewRoleParams addNewRoleParams,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        try
        {
            await _rolesService.AddRoleAsync(
                new(new(addNewRoleParams.Name),
                    addNewRoleParams.Claims),
                token);

            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            _logger.LogInformation("Request was processed with failed result");

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    /// <summary>
    /// Изменяет клэймы для существующей в системе роли.
    /// </summary>
    [HttpPut]
    [Authorize(Policy = nameof(ClaimType.ChangeRoleClaims))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("claims/change")]
    public async Task<IActionResult> ChangeRoleClaimsAsync(
        [Required][NotNull] ChangeRoleClaimsParams changeRoleClaimsParams,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        try
        {
            await _rolesService.ChangeRoleClaimsAsync(
                new(new(changeRoleClaimsParams.Name),
                    changeRoleClaimsParams.Claims),
                token);

            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            _logger.LogInformation("Request was processed with failed result");

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    /// <summary>
    /// Изменяет роль для указанного пользователя.
    /// </summary>
    [HttpPut]
    [Authorize(Policy = nameof(ClaimType.ChangeUserRole))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("users/change")]
    public async Task<IActionResult> ChangeUserRoleAsync(
        [Required][NotNull] ChangeUserRoleParams changeUserRoleParams,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        try
        {
            await _rolesService.ChangeUserRoleAsync(
                new(changeUserRoleParams.UserId),
                new(changeUserRoleParams.Name),
                token);

            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            _logger.LogInformation("Request was processed with failed result");

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    private readonly IRolesService _rolesService;
    private readonly ILogger<RolesController> _logger;
}