using BookHub.API.Authentification;
using BookHub.Contracts.REST.Responses.Account;
using BookHub.Contracts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookHub.Contracts.REST.Responses;
using BookHub.Contracts.REST.Requests;
using System.ComponentModel.DataAnnotations;

namespace BookHub.API.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class TestDataManipulatoinController : ControllerBase
{
    [HttpGet("data")]
    [ProducesResponseType<NewsItemsResponse<UserProfileInfoResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserProfileInfoResponse>> MeAsync(
        [FromBody][Required] DataManipulationContainerRequest requestContainer,
        CancellationToken token)
    {
        using var _ = _logger.BeginScope("{TraceId}", Guid.NewGuid());
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
}
