using BookHub.API;
using BookHub.API.Authentification;
using BookHub.API.Registrations;

using BooksService.Registrations;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .Configure<TestConfig>(builder.Configuration.GetRequiredSection(nameof(TestConfig)))
    .AddSingleton<IValidateOptions<TestConfig>, TestConfigValidator>()
    .AddScoped<IAuthorizationHandler, CustomAuthorizationHandler>();

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(
        opt => opt.SerializerSettings.Converters.Add(
            new StringEnumConverter(new SnakeCaseNamingStrategy())));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

builder.Services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
    })
    .AddSwaggerGenNewtonsoftSupport();

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
    .AddRequestsHandling()
    .AddPostgresStorage(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger().UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseRouting();
app.UseHealthChecks("/hc");

app.Run();