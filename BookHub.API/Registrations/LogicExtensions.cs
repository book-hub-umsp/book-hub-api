using BookHub.Abstractions.Logic.Converters;
using BookHub.Abstractions.Logic.Services;
using BookHub.API.Authentification;
using BookHub.Logic.Converters;
using BookHub.Logic.CrudServices;

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

    public static IServiceCollection AddRequestsHandling(
        this IServiceCollection services)
        => services
            .AddScoped<IBookDescriptionService, BookDescriptionService>()
            .AddSingleton<IBookParamsConverter, BookParamsConverter>();
}