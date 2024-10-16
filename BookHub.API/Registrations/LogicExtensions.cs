using BookHub.API.Authentification;
using BookHub.Logic.Configuration;
using Google.Apis.Auth;

using Microsoft.Extensions.Options;

namespace BookHub.API.Registrations;

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

    public static IServiceCollection AddRestOfConfigs(
        this IServiceCollection services,
        IConfiguration configuration)
       => services
            .Configure<BookTextConfiguration>(configuration.GetSection(nameof(BookTextConfiguration)))
            .AddSingleton<IValidateOptions<BookTextConfiguration>, BookTextConfigurationValidator>();
}