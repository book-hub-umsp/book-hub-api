using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookHub.API.Controllers;

/// <summary>
/// A controller for verifying user authentication.
/// </summary>
/// <remarks>
/// A JWT token can be generated for example on http://jwtbuilder.jamiekurtz.com/
/// filling out the issuer as https://accounts.google.com,
/// an audience can be taken from GoogleJsonWebTokenConfiguration: Audience.
/// </remarks>
[ApiController]
public class BaseController : Controller
{
    [HttpGet]
    [Route("hc")]
    public IActionResult GetHC() => Ok("You joined Books.Service index page. HC is true");

    
    [HttpGet]
    [Route("lk")]
    public IActionResult GetUserLK() => Ok("You joined Books.Service user page.");

    [Authorize(Policy = "Admin")]
    [HttpGet]
    [Route("adminLK")]
    public IActionResult GetAdminLK() => Ok("You joined Books.Service admin page.");
}