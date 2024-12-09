﻿using BookHub.API.Authentification;
using BookHub.API.Authentification.Configuration;
using BookHub.API.Authentification.Configuration.JWT;
using BookHub.Models.Account;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BookHub.API.Registrations;

public static class AuthentificationExtensions
{
    public static IServiceCollection AddAuth(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddScoped<IAuthorizationHandler, UserExistsAuthorizationHandler>()
            .AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>()

            .AddAuthProviders(configuration)
            .AddAuthorization(configuration)

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
                opt.TokenHandlers.Clear();

                opt.TokenHandlers.Add(new JsonWebTokenHandler());

                //opt.TokenValidationParameters.ValidIssuer = "https://accounts.google.com";

                // Todo: validate sign #81

                //opt.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
                //{
                //    OnTokenValidated = async (context) =>
                //    {
                //        try
                //        {
                //            var payload = await Google.Apis.Auth.GoogleJsonWebSignature
                //                .ValidateAsync(((JsonWebToken)context.SecurityToken).EncodedToken);

                //            context.Success();
                //        }
                //        catch (Google.Apis.Auth.InvalidJwtException)
                //        {
                //            context.Fail("Invalid token.");
                //        }
                //    }
                //};

                //opt.TokenValidationParameters.SignatureValidator =
                //    (token, _) => new JsonWebToken(token);

                opt.TokenValidationParameters.SignatureValidator =
                    (token, _) =>
                    {
                        //var payload = Google.Apis.Auth.GoogleJsonWebSignature.ValidateAsync(token).Result;
                        return new JsonWebToken(token);
                    };

                opt.TokenValidationParameters.ValidateIssuer = true;

                opt.TokenValidationParameters.ValidIssuer =
                    configuration.GetRequiredSection(nameof(AuthJWTConfiguration))
                        .GetRequiredSection(nameof(AuthJWTConfiguration.Google))
                        .GetRequiredSection(nameof(AuthJWTConfiguration.Google.Issuer))
                        .Value
                        ?? throw new InvalidOperationException("Auth configuration is invalid.");

                opt.TokenValidationParameters.ValidateAudience = true;

                opt.TokenValidationParameters.ValidAudience =
                    configuration.GetRequiredSection(nameof(AuthJWTConfiguration))
                        .GetRequiredSection(nameof(AuthJWTConfiguration.Google))
                        .GetRequiredSection(nameof(AuthJWTConfiguration.Google.Audience))
                        .Value
                        ?? throw new InvalidOperationException("Auth configuration is invalid.");
            })
            .AddJwtBearer(Auth.AuthProviders.YANDEX, opt =>
            {
                opt.TokenHandlers.Clear();

                opt.TokenHandlers.Add(new JsonWebTokenHandler());


                // Todo: validate sign #81
                opt.TokenValidationParameters.SignatureValidator =
                    (token, _) =>
                    {
                        //var payload = Google.Apis.Auth.GoogleJsonWebSignature.ValidateAsync(token).Result;
                        return new JsonWebToken(token);
                    };

                opt.TokenValidationParameters.ValidateIssuer = true;

                opt.TokenValidationParameters.ValidIssuer =
                    configuration.GetRequiredSection(nameof(AuthJWTConfiguration))
                        .GetRequiredSection(nameof(AuthJWTConfiguration.Yandex))
                        .GetRequiredSection(nameof(AuthJWTConfiguration.Yandex.Issuer))
                        .Value
                        ?? throw new InvalidOperationException("Auth configuration is invalid.");

                opt.TokenValidationParameters.ValidateAudience = true;

                opt.TokenValidationParameters.ValidAudience =
                    configuration.GetRequiredSection(nameof(AuthJWTConfiguration))
                        .GetRequiredSection(nameof(AuthJWTConfiguration.Yandex))
                        .GetRequiredSection(nameof(AuthJWTConfiguration.Yandex.Audience))
                        .Value
                        ?? throw new InvalidOperationException("Auth configuration is invalid.");
            })
            .Services;

    private static IServiceCollection AddAuthorization(
        this IServiceCollection services,
        IConfiguration configuration)
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
                opt => opt
                    .AddRequirements(new PermissionRequirement(permission)));
        }

        return authorizationBuilder.Services;
    }
}