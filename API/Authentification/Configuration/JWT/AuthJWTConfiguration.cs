using System.ComponentModel.DataAnnotations;

using Microsoft.Extensions.Options;

namespace BookHub.API.Service.Authentification.Configuration.JWT;

/// <summary>
/// Конфигурация аутентификации JWT.
/// </summary>
public sealed class AuthJWTConfiguration
{
    /// <summary>
    /// Конфигурация для Google.
    /// </summary>
    [Required]
    [ValidateObjectMembers]
    public required GoogleConfiguration Google { get; init; }

    /// <summary>
    /// Конфигурация для Yandex.
    /// </summary>
    [Required]
    [ValidateObjectMembers]
    public required YandexConfiguration Yandex { get; init; }
}
