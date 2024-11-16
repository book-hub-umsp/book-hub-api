using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using BookHub.API.Roles;
using BookHub.Contracts.REST.Requests.Account.Roles;
using BookHub.Models.Account;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookHub.API.Controllers;

/// <summary>
/// Контроллер для администратора.
/// </summary>
[Authorize]
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public sealed class AdminController : ControllerBase
{
    /// <summary>
    /// Добавляет новую роль в системе.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = Claims.ADD_NEW_ROLE)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("roles/add")]
    public Task<IActionResult> AddNewRoleAsync(
        [Required][NotNull] AddNewRoleParams addNewRoleParams,
        CancellationToken token)
    {
        return Ok();
    }

    /// <summary>
    /// Изменяет клэймы для существующей в системе роли.
    /// </summary>
    [HttpPut]
    [Authorize(Policy = Claims.CHANGE_ROLE_CLAIMS)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("roles/changeClaims")]
    public IActionResult ChangeRoleClaims()
    {
        return Ok();
    }

    /// <summary>
    /// Изменяет роль для указанного пользователя.
    /// </summary>
    [HttpPut]
    [Authorize(Policy = Claims.CHANGE_USER_ROLE)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Route("users/{userId}/changeRole/{newRole}")]
    public IActionResult ChangeUserRole(
        [FromRoute] long userId,
        [FromRoute] string newRole)
    {
        return Ok();
    }
}