using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookHub.API.Controllers;

/// <summary>
/// Тестовый контроллер авторизации.
/// </summary>
/// <remarks>
/// Собирать можно здесь: http://jwtbuilder.jamiekurtz.com/
/// </remarks>
[ApiController]
[Route("[controller]")]
public class TestAuthController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    [Route("any")]
    public IActionResult AnyAllowEndpoint() => Ok("Any allow endpoint is works!");

    [HttpGet]
    [Authorize]
    [Route("jwt")]
    public IActionResult AnyJWTEndpoint() => Ok("Any JWT endpoint is works!");

    [HttpGet]
    [Authorize(Policy = "SpecialJWT")]
    [Route("special")]
    public IActionResult SpecialJWTEndpoint() => Ok("Special JWT endpoint is works!");
}
