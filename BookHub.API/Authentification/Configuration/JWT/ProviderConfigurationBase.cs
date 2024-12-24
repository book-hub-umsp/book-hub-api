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

    /// <summary>
    /// Validate signature (issuer).
    /// </summary>
    [Required]
    public required bool ValidateIssuer { get; init; }

    /// <summary>
    /// Validate audience.
    /// </summary>
    [Required]
    public required bool ValidateAudience { get; init; }

    /// <summary>
    /// Validate signature.
    /// </summary>
    [Required]
    public required bool ValidateSignature { get; init; }
}