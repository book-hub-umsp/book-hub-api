using BookHub.Models;
using BookHub.Models.Users;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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
    private readonly TestConfig _config;

    public TestAuthController(
        IOptions<TestConfig> options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _config = options.Value;
    }   

    [HttpGet]
    [AllowAnonymous]
    [Route("any")]
    public IActionResult AnyAllowEndpoint() => Ok($"Any allow endpoint is works! Config: {_config.Content}");

    [HttpGet]
    [Authorize]
    [Route("jwt")]
    public IActionResult AnyJWTEndpoint() => Ok($"Any JWT endpoint is works! Config: {_config.Content}");

    [HttpGet]
    [Authorize(Policy = "SpecialJWT")]
    [Route("special")]
    public IActionResult SpecialJWTEndpoint() => Ok($"Special JWT endpoint is works! Config: {_config.Content}");
}
