using System.IdentityModel.Tokens.Jwt;

namespace BookHub.Abstractions.Logic.Services.Auth;

/// <summary>
/// Описывает сервис по валидации подписей Yandex авторизации.
/// </summary>
public interface IYandexAuthService
{
    /// <summary>
    /// Валидирует текущий авторизационный токен для Yandex.
    /// </summary>
    /// <param name="encodedToken">
    /// Токен авторизации.
    /// </param>
    /// <param name="token">
    /// Токен отмены.
    /// </param>
    /// <returns>
    /// <see cref="JwtPayload"/> со списком клэймов.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Если <paramref name="encodedToken"/> пуст или <see langword="null"/>.
    /// </exception>
    public Task<JwtPayload> ValidateYandexTokenAsync(
        string encodedToken, 
        CancellationToken token);
}