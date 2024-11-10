﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using BookHub.Abstractions.Logic.Services;
using BookHub.Contracts.REST.Requests.Admins;
using BookHub.Contracts.REST.Responces;
using BookHub.Models.CRUDS.Requests.Admins;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookHub.API.Controllers;

/// <summary>
/// Контроллер действий администратора.
/// </summary>
[ApiController]
[Route("admin")]
[Authorize]
[Produces("application/json")]
public sealed class AdminActionsController : ControllerBase
{
    public AdminActionsController(
        IAdminActionsService service,
        ILogger<AdminActionsController> logger)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPatch("user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateUserRoleAsync(
        [Required][NotNull] UpdateUserRoleRequest roleRequest,
        CancellationToken token)
    {
        _logger.LogInformation("Start handling admin update user role request");

        try
        {
            await _service.UpdateUserRoleAsync(
                new(new(roleRequest.AdminId), 
                    new(roleRequest.ModifiedUserId), 
                    roleRequest.NewRole),
                token);

            _logger.LogInformation("Request handled succesfully");

            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            _logger.LogInformation("Request was processed with failed result");

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    private readonly IAdminActionsService _service;
    private readonly ILogger<AdminActionsController> _logger;
}