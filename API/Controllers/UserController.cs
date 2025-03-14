using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using BookHub.API.Abstractions;
using BookHub.API.Abstractions.Logic.Services.Account;
using BookHub.API.Contracts;
using BookHub.API.Contracts.REST.Requests.Account;
using BookHub.API.Contracts.REST.Responses.Account;
using BookHub.API.Models;
using BookHub.API.Models.Account;
using BookHub.API.Models.DomainEvents.Account;
using BookHub.API.Models.Identifiers;
using BookHub.API.Service.Authentification;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookHub.API.Service.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public sealed class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserIdentityFacade _userIdentityFacade;
    private readonly ILogger<UserController> _logger;

    public UserController(
        IUserService userService,
        IUserIdentityFacade userIdentityFacade,
        ILogger<UserController> logger)
    {
        ArgumentNullException.ThrowIfNull(userService);
        _userService = userService;

        ArgumentNullException.ThrowIfNull(userIdentityFacade);
        _userIdentityFacade = userIdentityFacade;

        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    /// <summary>
    /// Возвращает информацию о профиле авторизованного пользователя.
    /// </summary>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// <see cref="ActionResult{TValue}"/> с данными профиля пользователя.
    /// </returns>
    [Authorize(Policy = Auth.Policies.ALLOW_REGISTER_POLICY)]
    [HttpGet("me")]
    [ProducesResponseType<UserProfileInfoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserProfileInfoResponse>> MeAsync(CancellationToken token)
    {
        _logger.LogDebug("Getting info by user id: {Id}", _userIdentityFacade.Id!.Value);

        try
        {
            var profileInfo = await _userService.GetUserProfileInfoAsync(_userIdentityFacade.Id, token);

            return Ok(UserProfileInfoResponse.FromDomain(profileInfo));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    /// <summary>
    /// Возвращает информацию о профиле запрашиваемого пользователя.
    /// </summary>
    /// <param name="userId">
    /// Идентификатор пользователя.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// <see cref="ActionResult{TValue}"/> с данными профиля пользователя.
    /// </returns>
    /// <response code="400">
    /// Когда пользователя с таким идентификатором не удалось найти.
    /// </response>
    [AllowAnonymous]
    [HttpGet("{userId}")]
    [ProducesResponseType<UserProfileInfoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserProfileInfoResponse>> GetUserByIdAsync(
        [FromRoute] long userId,
        CancellationToken token)
    {
        _logger.LogDebug("Getting info by user id: {Id}", userId);

        try
        {
            var profileInfo = await _userService.GetUserProfileInfoAsync(new(userId), token);

            return Ok(UserProfileInfoResponse.FromDomain(profileInfo));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    /// <summary>
    /// Обновляет информацию о профиле авторизированного пользователя.
    /// </summary>
    /// <param name="request">
    /// Запрос на обновление.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// <see cref="IActionResult"/>.
    /// </returns>
    [Authorize]
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateMyProfileInfoAsync(
        [Required][NotNull] UpdateUserProfileInfoRequest request,
        CancellationToken token)
    {
        _logger.LogDebug("Update info by user id: {Id}", _userIdentityFacade.Id!.Value);

        try
        {
            var updated = FromRequest(_userIdentityFacade.Id, request);

            await _userService.UpdateUserInfoAsync(updated, token);

            return Ok();
        }
        catch (Exception ex)
        when (ex is InvalidOperationException or ArgumentException)
        {
            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    private static UserUpdatedBase FromRequest(
        Id<User> userId,
        UpdateUserProfileInfoRequest request)
    {
        return (request.NewName, request.About) switch
        {
            (not null, null) => new UserUpdated<Name<User>>(userId, new(request.NewName)),

            (null, not null) => new UserUpdated<About>(userId, new(request.About)),

            _ => throw new InvalidOperationException("Update parameters is invalid.")
        };
    }
}
