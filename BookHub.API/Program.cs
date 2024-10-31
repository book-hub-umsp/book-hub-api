using BookHub.API;
using BookHub.API.Authentification;
using BookHub.API.Registrations;

using BooksService.Registrations;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .Configure<TestConfig>(builder.Configuration.GetRequiredSection(nameof(TestConfig)))
    .AddSingleton<IValidateOptions<TestConfig>, TestConfigValidator>()
    .AddScoped<IAuthorizationHandler, CustomAuthorizationHandler>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

builder.Services
    .AddCors(options => options
        .AddDefaultPolicy(builder => _ = builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin()));

builder.Services
    .AddAuthentication()
    .AddJwtBearer(opt =>
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

        //opt.TokenValidationParameters.ValidateAudience = false;

        // opt.TokenValidationParameters.ValidateIssuer = false;

        opt.TokenValidationParameters.SignatureValidator =
            (token, _) => new JsonWebToken(token);

        //opt.TokenValidationParameters.ValidAudiences =
        //    builder.Configuration.GetSection("GoogleJsonWebTokenConfiguration:Audience")
        //        .AsEnumerable()
        //        .Select(v => v.Value)
        //        .Where(v => v is not null);
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(
        "SpecialJWT",
        b => b
            .RequireClaim(JwtRegisteredClaimNames.Iss, "BookHub")
            .AddRequirements(new CustomRequirementMarker()))
    .AddPolicy(
        "Admin",
        b => b.RequireClaim(
            JwtRegisteredClaimNames.Email,
            builder.Configuration.GetSection(nameof(AdminAuthorizationConfiguration))
                .Get<AdminAuthorizationConfiguration>()?.Admins ??
                    throw new InvalidOperationException(
                        "Admin authorization configuration is not found.")));

builder.Services
    .AddAuthorizationConfigs(builder.Configuration)
    .AddPostgresStorage(builder.Configuration);

var app = builder.Build();

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHealthChecks("/hc");

app.Run();
