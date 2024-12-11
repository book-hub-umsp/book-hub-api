using BookHub.Abstractions.Logic.Services.Auth;
using BookHub.API.Authentification.Configuration.JWT;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json;

using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;

namespace BookHub.API.Authentification;

/// <summary>
/// Представляет сервис по валидации подписей Yandex авторизации.
/// </summary>
public sealed class YandexAuthService : IYandexAuthService
{
    public YandexAuthService(
        HttpClient httpClient,
        IOptions<YandexConfiguration> options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        ArgumentNullException.ThrowIfNull(options);
        _configuration = options.Value;
    }

    public Task<JwtPayload> ValidateYandexTokenAsync(
        string encodedToken,
        CancellationToken token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(encodedToken);

        token.ThrowIfCancellationRequested();

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = _configuration.ValidateIssuer,
            ValidIssuer = _configuration.Issuer,
            ValidateAudience = _configuration.ValidateAudience,
            ValidAudience = _configuration.Audience,
            IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
            {
                var keys = GetYandexPublicKeysAsync().GetAwaiter().GetResult();

                return keys.Select(key => new X509SecurityKey(key));
            }
        };

        var tokenHandler = new JsonWebTokenHandler();

        var result = tokenHandler.ValidateToken(encodedToken, validationParameters);

        if (!result.IsValid)
        {
            throw new SecurityTokenException(
                $"Invalid Yandex token: {result.Exception?.Message}");
        }

        var payload = new JwtPayload();

        foreach (var claim in result.Claims)
        {
            payload.Add(claim.Key, claim.Value);
        }

        return Task.FromResult(payload);
    }

    private async Task<IReadOnlyCollection<X509Certificate2>> GetYandexPublicKeysAsync()
    {
        var openIdConfig = await FindOpenIdConfigurationAsync();

        var jwks = await FindJsonWebKeySetAsync(openIdConfig);

        return jwks.Keys
            .Select(key => new X509Certificate2(
                Convert.FromBase64String(key.X5c.First())))
            .ToList();
    }

    private async Task<OpenIdConfiguration> FindOpenIdConfigurationAsync()
    {
        var openIdConfigResponse = await _httpClient.GetAsync(YANDEX_VALIDATION_URL);

        openIdConfigResponse.EnsureSuccessStatusCode();

        var openIdConfigJson = await openIdConfigResponse.Content.ReadAsStringAsync();

        using var reader = new JsonTextReader(new StreamReader(openIdConfigJson));

        var openIdConfig = _deserializer.Deserialize<OpenIdConfiguration>(reader);

        Debug.Assert(openIdConfig is not null);

        return openIdConfig;
    }

    private async Task<JsonWebKeySet> FindJsonWebKeySetAsync(
        OpenIdConfiguration openIdConfig)
    {
        var jwksUrl = openIdConfig.JwksUri;

        var jwksResponse = await _httpClient.GetAsync(jwksUrl);

        jwksResponse.EnsureSuccessStatusCode();

        var jwksJson = await jwksResponse.Content.ReadAsStringAsync();

        using var reader = new JsonTextReader(new StreamReader(jwksJson));

        var jwks = _deserializer.Deserialize<JsonWebKeySet>(reader);

        Debug.Assert(jwks is not null);

        return jwks;
    }

    private class OpenIdConfiguration
    {
        [JsonProperty("jwks_uri")]
        public required string JwksUri { get; init; }
    }

    private class JsonWebKeySet
    {
        [JsonProperty("keys")]
        public required IReadOnlyCollection<JsonWebKey> Keys { get; init; }
    }

    private class JsonWebKey
    {
        [JsonProperty("x5c")]
        public required IReadOnlyCollection<string> X5c { get; init; }
    }

    private readonly HttpClient _httpClient;
    private readonly YandexConfiguration _configuration;

    private readonly JsonSerializer _deserializer = JsonSerializer.CreateDefault();

    private const string YANDEX_VALIDATION_URL = "https://login.yandex.ru/.well-known/openid-configuration";
}