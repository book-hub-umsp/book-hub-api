using System.ComponentModel.DataAnnotations;

namespace BookHub.API.Authentification.Configuration.JWT;

/// <summary>
/// Базовая конфигурация поставщика.
/// </summary>
public abstract class ProviderConfigurationBase
{
    /// <summary>
    /// Audience.
    /// </summary>
    /// <remarks>
    /// <see href="https://datatracker.ietf.org/doc/html/rfc7519#section-4.1.3"/>
    /// </remarks>
    [Required]
    public required string Audience { get; init; }

    /// <summary>
    /// Issuer.
    /// </summary>
    /// <remarks>
    /// <see href="https://datatracker.ietf.org/doc/html/rfc7519#section-4.1.1"/>
    /// </remarks>
    [Required]
    public required string Issuer { get; init; }
}
