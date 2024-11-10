using System.ComponentModel.DataAnnotations;

namespace BookHub.API.Authentification.Configuration.JWT;

public abstract class ProviderConfigurationBase
{
    [Required]
    public required string Audience { get; init; }

    [Required]
    public required string Issuer { get; init; }
}
