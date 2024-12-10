using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;

using BookHub.API.Authentification.Configuration.JWT;

using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json;

namespace BookHub.API.Registrations;

/// <summary>
/// Вспомогательный метод для проверки публичных ключей Yandex.
/// </summary>
public static class YandexAuthHelper
{
    /// <remarks>
    /// https://learn.microsoft.com/en-us/dotnet/api/microsoft.identitymodel.jsonwebtokens.jsonwebtokenhandler.validatetoken?view=msal-web-dotnet-latest.
    /// </remarks>
    [Obsolete]
    public async static Task<JwtPayload> ValidateYandexTokenAsync(
        YandexConfiguration config,
        string encodedToken)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = config.ValidateIssuer,
            ValidIssuer = config.Issuer,
            ValidateAudience = config.ValidateAudience,
            ValidAudience = config.Audience,
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

        return payload;
    }

    private static async Task<IReadOnlyCollection<X509Certificate2>> GetYandexPublicKeysAsync()
    {
        using var httpClient = new HttpClient();

        const string openIdConfigUrl = "https://login.yandex.ru/.well-known/openid-configuration";
        var openIdConfigResponse = await httpClient.GetAsync(openIdConfigUrl);
        openIdConfigResponse.EnsureSuccessStatusCode();
        var openIdConfigJson = await openIdConfigResponse.Content.ReadAsStringAsync();

        var openIdConfig = JsonConvert.DeserializeObject<OpenIdConfiguration>(openIdConfigJson);

        Debug.Assert(openIdConfig is not null);

        var jwksUrl = openIdConfig.JwksUri;

        var jwksResponse = await httpClient.GetAsync(jwksUrl);
        jwksResponse.EnsureSuccessStatusCode();
        var jwksJson = await jwksResponse.Content.ReadAsStringAsync();
        var jwks = JsonConvert.DeserializeObject<JsonWebKeySet>(jwksJson);

        Debug.Assert(jwks is not null);

        return jwks.Keys
            .Select(key => new X509Certificate2(
                Convert.FromBase64String(key.X5c.First())))
            .ToList();
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
}