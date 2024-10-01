using BooksService.Authentification;

using Google.Apis.Auth;

using Microsoft.Extensions.Options;

namespace BooksService.Registrations;

internal static class LogicExtensions
{
    public static IServiceCollection AddAuthorizationConfigs(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddSingleton<IValidateOptions<GoogleJsonWebSignature.ValidationSettings>, GoogleJsonWebTokenConfigurationValidator>()
            .AddOptions<GoogleJsonWebSignature.ValidationSettings>()
                .Bind(configuration.GetSection("GoogleJsonWebTokenConfiguration"))
                .ValidateOnStart()
                .Services
            .AddSingleton<IValidateOptions<AdminAuthorizationConfiguration>, AdminAuthorizationConfigurationValidator>()
            .AddOptions<AdminAuthorizationConfiguration>()
                .Bind(configuration.GetSection(nameof(AdminAuthorizationConfiguration)))
                .ValidateOnStart()
                .Services;
}