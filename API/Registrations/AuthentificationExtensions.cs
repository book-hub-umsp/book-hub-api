using BookHub.API.Abstractions.Logic.Services.Auth;
using BookHub.API.Authentification.Configuration;
using BookHub.API.Models.Account;
using BookHub.API.Service.Authentification;
using BookHub.API.Service.Authentification.Configuration.JWT;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace BookHub.API.Service.Registrations;

internal static class AuthentificationExtensions
{
    public static IServiceCollection AddAuth(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddScoped<IAuthorizationHandler, UserExistsAuthorizationHandler>()
            .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>()

            .AddScoped<IYandexAuthService, YandexAuthService>()
            .Configure<YandexConfiguration>(
                configuration
                    .GetRequiredSection(nameof(AuthJWTConfiguration))
                    .GetSection(nameof(AuthJWTConfiguration.Yandex)))

            .AddAuthProviders(configuration)
            .AddAuthorization()

            .AddSingleton<IValidateOptions<AuthJWTConfiguration>, AuthJWTConfigurationValidator>()
            .AddOptionsWithValidateOnStart<AuthJWTConfiguration>()
            .Bind(configuration.GetRequiredSection(nameof(AuthJWTConfiguration)))

            .Services;

    private static IServiceCollection AddAuthProviders(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddAuthentication()
            .AddJwtBearer(Auth.AuthProviders.GOOGLE, opt =>
            {
                var googleConfig = configuration.GetSection(nameof(AuthJWTConfiguration))
                    .GetSection(nameof(AuthJWTConfiguration.Google))
                    .Get<GoogleConfiguration>()
                    ?? throw new InvalidOperationException("Auth configuration is invalid.");

                opt.TokenHandlers.Clear();

                opt.TokenHandlers.Add(new JsonWebTokenHandler());

                opt.TokenValidationParameters.ValidateIssuer = googleConfig.ValidateIssuer;

                opt.TokenValidationParameters.ValidIssuer = googleConfig.Issuer;

                opt.TokenValidationParameters.ValidateAudience = googleConfig.ValidateAudience;

                opt.TokenValidationParameters.ValidAudience = googleConfig.Audience;

                opt.TokenValidationParameters.SignatureValidator =
                   (token, _) =>
                       //var payload = Google.Apis.Auth.GoogleJsonWebSignature.ValidateAsync(token).Result;
                       new JsonWebToken(token);

                opt.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
                {
                    OnTokenValidated = async (context) =>
                    {
                        try
                        {
                            if (googleConfig.ValidateSignature)
                            {
                                var payload = await Google.Apis.Auth.GoogleJsonWebSignature
                                    .ValidateAsync(((JsonWebToken)context.SecurityToken).EncodedToken);
                            }

                            context.Success();
                        }
                        catch (Google.Apis.Auth.InvalidJwtException ex)
                        {
                            context.Fail($"Invalid token: {ex.Message}.");
                        }
                    },
                    OnAuthenticationFailed = context => Task.CompletedTask
                };
            })
            .AddJwtBearer(Auth.AuthProviders.YANDEX, opt =>
            {
                var yandexConfig = configuration.GetSection(nameof(AuthJWTConfiguration))
                    .GetSection(nameof(AuthJWTConfiguration.Yandex))
                    .Get<YandexConfiguration>()
                    ?? throw new InvalidOperationException("Auth configuration is invalid.");

                opt.TokenHandlers.Clear();

                opt.TokenHandlers.Add(new JsonWebTokenHandler());

                opt.TokenValidationParameters.ValidateIssuer = yandexConfig.ValidateIssuer;

                opt.TokenValidationParameters.ValidIssuer = yandexConfig.Issuer;

                opt.TokenValidationParameters.ValidateAudience = yandexConfig.ValidateAudience;

                opt.TokenValidationParameters.ValidAudience = yandexConfig.Audience;

                opt.TokenValidationParameters.SignatureValidator =
                   (token, _) =>
                       //var payload = Google.Apis.Auth.GoogleJsonWebSignature.ValidateAsync(token).Result;
                       new JsonWebToken(token);

                opt.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
                {
                    OnTokenValidated = async (context) =>
                    {
                        try
                        {
                            if (yandexConfig.ValidateSignature)
                            {
                                var serviceProvider = context.HttpContext.RequestServices;

                                var yandexAuthService = serviceProvider.GetRequiredService<IYandexAuthService>();

                                // some logic is duplicated here
                                var payload = await yandexAuthService.ValidateYandexTokenAsync(
                                    ((JsonWebToken)context.SecurityToken).EncodedToken,
                                    CancellationToken.None);
                            }

                            context.Success();
                        }
                        catch (SecurityTokenException ex)
                        {
                            context.Fail($"Invalid token: {ex.Message}.");
                        }
                    },
                    OnAuthenticationFailed = context => Task.CompletedTask
                };

            })
            .Services;

    private static IServiceCollection AddAuthorization(
        this IServiceCollection services)
        => services
            .AddAuthorizationBuilder()
            .AddDefaultPolicy(
                Auth.Policies.DEFAULT_POLICY,
                opt => opt
                    .AddRequirements(new UserExistsRequirementMarker())
                    .AddAuthenticationSchemes([Auth.AuthProviders.GOOGLE, Auth.AuthProviders.YANDEX]))
            .AddPolicy(
                Auth.Policies.ALLOW_REGISTER_POLICY,
                opt => opt
                    .AddRequirements(new UserExistsRequirementMarker(NeedRegisterIfNotExists: true))
                    .AddAuthenticationSchemes([Auth.AuthProviders.GOOGLE, Auth.AuthProviders.YANDEX]))
            .AddPolicy(
                Auth.AuthProviders.GOOGLE,
                opt => opt
                    .AddRequirements(new UserExistsRequirementMarker())
                    .AddAuthenticationSchemes([Auth.AuthProviders.GOOGLE]))
            .AddPolicy(
                Auth.AuthProviders.YANDEX,
                opt => opt
                    .AddRequirements(new UserExistsRequirementMarker())
                    .AddAuthenticationSchemes([Auth.AuthProviders.YANDEX]))
            .AddPermissionPolicies();

    private static IServiceCollection AddPermissionPolicies(
        this AuthorizationBuilder authorizationBuilder)
    {
        foreach (var permission in Enum.GetValues<Permission>())
        {
            _ = authorizationBuilder.AddPolicy(
                    permission.ToString(),
                    opt => opt.AddRequirements(new PermissionRequirement(permission)));
        }

        return authorizationBuilder.Services;
    }
}