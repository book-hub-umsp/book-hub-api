﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using BookHub.Abstractions;
using BookHub.API.Authentification;
using BookHub.Contracts;
using BookHub.Contracts.REST.Requests.Account;
using BookHub.Contracts.REST.Responses.Account;
using BookHub.Logic.Converters.Account;
using BookHub.Logic.Services.Account;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookHub.API.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public sealed class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserRequestConverter _converter;
    private readonly IUserIdentityFacade _userIdentityFacade;
    private readonly ILogger<UserController> _logger;

    public UserController(
        IUserService userService,
        IUserRequestConverter converter,
        IUserIdentityFacade userIdentityFacade,
        ILogger<UserController> logger)
    {
        ArgumentNullException.ThrowIfNull(userService);
        _userService = userService;

        ArgumentNullException.ThrowIfNull(converter);
        _converter = converter;

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
        _logger.LogInformation("Getting info by user id: {Id}", _userIdentityFacade.Id!.Value);

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
        _logger.LogInformation("Getting info by user id: {Id}", userId);

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
        _logger.LogInformation("Update info by user id: {Id}", _userIdentityFacade.Id!.Value);

        try
        {
            var updated = _converter.Convert(_userIdentityFacade.Id, request);

            await _userService.UpdateUserInfoAsync(updated, token);

            return Ok();
        }
        catch (Exception ex)
        when (ex is InvalidOperationException or ArgumentException)
        {
            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }
}
