using BookHub.API.Authentification.Configuration.JWT;

using Microsoft.Extensions.Options;

namespace BookHub.API.Authentification.Configuration;

/// <summary>
/// Валидатор для <see cref="AuthJWTConfiguration"/>.
/// </summary>
[OptionsValidator]
public sealed partial class AuthJWTConfigurationValidator : 
    IValidateOptions<AuthJWTConfiguration>;