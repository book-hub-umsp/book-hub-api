using BookHub.Abstractions.Logic.Services.Auth;
using BookHub.API.Authentification;
using BookHub.API.Registrations;

using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddNewtonsoftJson(
        opt => opt.SerializerSettings.Converters.Add(
            new StringEnumConverter(new SnakeCaseNamingStrategy())));

builder.Services.AddHealthChecks();

builder.Services
    .AddCors(options => options
        .AddDefaultPolicy(builder => _ = builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowAnyOrigin()));

builder.Services
    .AddSwagger()
    .AddAuth(builder.Configuration)
    .AddLogic(builder.Configuration)
    .AddPostgresStorage(builder.Environment, builder.Configuration)
    .AddHttpClient<IYandexAuthService, YandexAuthService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // app.UseSwagger().UseSwaggerUI();
}

app.UseSwagger().UseSwaggerUI();

app.UseCors();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHealthChecks("/hc");

app.Run();
