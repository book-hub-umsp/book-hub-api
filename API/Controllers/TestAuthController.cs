using BookHub.API.Service.Authentification;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BookHub.API.Service.Controllers;

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
    [AllowAnonymous]
    [Route("test-https")]
    public IActionResult TestHttpsEndpoint()
    {
        var isHttps = Request.IsHttps; // Checks if the original request was HTTPS
        var scheme = Request.Scheme;   // Shows HTTP or HTTPS
        var forwardedProto = Request.Headers["X-Forwarded-Proto"].ToString(); // From Traefik

        return Ok(new
        {
            IsHttps = isHttps,
            Scheme = scheme,
            XForwardedProto = forwardedProto
        });
    }

    [HttpGet]
    [Authorize]
    [Route("jwt")]
    public IActionResult AnyJWTEndpoint() => Ok($"Any JWT endpoint is works! Config: {_config.Content}");

    [HttpGet]
    [Authorize(Policy = Auth.AuthProviders.GOOGLE)]
    [Route("jwt/google")]
    public IActionResult GoogleJWTEndpoint() => Ok($"Any JWT endpoint is works! Config: {_config.Content}");

    [HttpGet]
    [Authorize(Policy = Auth.AuthProviders.YANDEX)]
    [Route("jwt/yandex")]
    public IActionResult YandexJWTEndpoint() => Ok($"Any JWT endpoint is works! Config: {_config.Content}");
}
