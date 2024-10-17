using BookHub.API.Authentification;
using BookHub.API.Registrations;
using Microsoft.IdentityModel.JsonWebTokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

builder.Services
    .AddCors(options =>
    {
        options.AddDefaultPolicy(
            builder =>
            {
                _ = builder.AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowAnyOrigin();
            });
    });

builder.Services
    .AddAuthentication()
    .AddJwtBearer(opt =>
    {
        opt.TokenHandlers.Clear();

        opt.TokenHandlers.Add(new JsonWebTokenHandler());

        opt.TokenValidationParameters.ValidIssuer = "https://accounts.google.com";

        opt.TokenValidationParameters.SignatureValidator =
            (token, _) => new JsonWebToken(token);

        opt.TokenValidationParameters.ValidAudiences =
            builder.Configuration.GetSection("GoogleJsonWebTokenConfiguration:Audience")
                .AsEnumerable()
                .Select(v => v.Value)
                .Where(v => v is not null);
    });

builder.Services.AddAuthorizationBuilder()
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
    .AddRestOfConfigs(builder.Configuration);

var app = builder.Build();

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHealthChecks("/hc");

app.Run();
