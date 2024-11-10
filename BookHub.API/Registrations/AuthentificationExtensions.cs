using BookHub.API.Authentification;

using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BookHub.API.Registrations;

public static class AuthentificationExtensions
{
    public static IServiceCollection AddAuth(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddScoped<IAuthorizationHandler, UserExistsAuthorizationHandler>()
            .AddAuthProviders(configuration)
            .AddAuthorization(configuration);

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

                //opt.TokenValidationParameters.ValidAudience = "bookhub";

                // Validate signature

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

                opt.TokenValidationParameters.ValidateAudience = false;

                opt.TokenValidationParameters.ValidateIssuer = false;

                opt.TokenValidationParameters.SignatureValidator =
                    (token, _) => new JsonWebToken(token);

                //opt.TokenValidationParameters.ValidAudiences =
                //    builder.Configuration.GetSection("GoogleJsonWebTokenConfiguration:Audience")
                //        .AsEnumerable()
                //        .Select(v => v.Value)
                //        .Where(v => v is not null);
            })
            .Services;

    private static IServiceCollection AddAuthorization(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddAuthorizationBuilder()
            .AddDefaultPolicy("DefaultPolicy", opt => opt.AddRequirements(new UserExistsRequirementMarker()))
            .AddPolicy(
                "SpecialJWT",
                b => b
                    .RequireClaim(JwtRegisteredClaimNames.Iss, "BookHub")
                    .AddRequirements(new UserExistsRequirementMarker()))
            //.AddPolicy(
            //    "Admin",
            //    b => b.RequireClaim(
            //        JwtRegisteredClaimNames.Email,
            //        configuration.GetSection(nameof(AdminAuthorizationConfiguration))
            //            .Get<AdminAuthorizationConfiguration>()?.Admins ??
            //                throw new InvalidOperationException(
            //                    "Admin authorization configuration is not found.")))
            .Services;

    //private static IServiceCollection AddAuthorizationConfigs(
    //    this IServiceCollection services,
    //    IConfiguration configuration)
    //    => services
    //        .AddSingleton<IValidateOptions<GoogleJsonWebSignature.ValidationSettings>, GoogleJsonWebTokenConfigurationValidator>()
    //        .AddOptions<GoogleJsonWebSignature.ValidationSettings>()
    //            .Bind(configuration.GetSection("GoogleJsonWebTokenConfiguration"))
    //            .ValidateOnStart()
    //            .Services
    //        .AddSingleton<IValidateOptions<AdminAuthorizationConfiguration>, AdminAuthorizationConfigurationValidator>()
    //        .AddOptions<AdminAuthorizationConfiguration>()
    //            .Bind(configuration.GetSection(nameof(AdminAuthorizationConfiguration)))
    //            .ValidateOnStart()
    //            .Services;
}
