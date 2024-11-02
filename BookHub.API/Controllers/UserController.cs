using BookHub.Abstractions;
using BookHub.Contracts.REST.Responces;
using BookHub.Contracts.REST.Responces.Users;
using BookHub.Logic.Services.Users;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookHub.API.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public sealed class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IHttpUserIdentityFacade _userIdentityFacade;
    private readonly ILogger<UserController> _logger;

    public UserController(
        IUserService userService,
        IHttpUserIdentityFacade userIdentityFacade,
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
    /// Возвращет информацию о профиле авторизованного пользователя.
    /// </summary>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// <see cref="ActionResult{TValue}"/> с данными профиля пользователя.
    /// </returns>
    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType<UserProfileInfoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserProfileInfoResponse>> MeAsync(CancellationToken token)
    {
        _logger.LogInformation("{Id}", _userIdentityFacade.Id!.Value);

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
    /// Возвращет информацию о профиле запрашиваемого пользователя.
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
    [HttpGet("{userId}")]
    [ProducesResponseType<UserProfileInfoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserProfileInfoResponse>> GetUserByIdAsync(
        [FromRoute] long userId,
        CancellationToken token)
    {
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

    [Authorize]
    [HttpPost("update")]
    [ProducesResponseType<UserProfileInfoResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserProfileInfoResponse>> UpdateMyInfoAsync(
        CancellationToken token)
    {
        _logger.LogInformation("{Id}", _userIdentityFacade.Id!.Value);

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
}
