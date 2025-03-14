using System.Runtime.InteropServices;

using BookHub.API.Abstractions.Logic.Services.Account;
using BookHub.API.Contracts;
using BookHub.API.Contracts.REST.Requests;
using BookHub.API.Contracts.REST.Responses;
using BookHub.API.Contracts.REST.Responses.Account;

using Microsoft.AspNetCore.Mvc;

namespace BookHub.API.Service.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public sealed class TestDataManipulationController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<TestDataManipulationController> _logger;

    public TestDataManipulationController(
        IUserService userService,
        ILogger<TestDataManipulationController> logger)
    {
        ArgumentNullException.ThrowIfNull(userService);
        _userService = userService;

        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    [HttpGet("test")]
    [ProducesResponseType<NewsItemsResponse<UserProfileInfoResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<NewsItemsResponse<UserProfileInfoResponse>>> TestAsync(
        [FromBody][Optional] DataManipulationRequest request,
        CancellationToken token)
    {
        _logger.LogDebug("Request parsing result: @{Request}", request);

        try
        {
            var users = await _userService.GetUserProfilesInfoAsync(
                DataManipulationRequest.ToDomain(request),
                token);

            return Ok(
                NewsItemsResponse<UserProfileInfoResponse>.FromDomain(
                    users,
                    UserProfileInfoResponse.FromDomain));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }
}
