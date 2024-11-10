namespace BookHub.API.Authentification;

/// <summary>
/// Содержит константы для описания аутентификации/авторизации.
/// </summary>
public struct Auth
{
    /// <summary>
    /// Содержит константы провайдеров аутентификации.
    /// </summary>
    public struct AuthProviders
    {
        public const string GOOGLE = "google";

        public const string YANDEX = "yandex";
    }

    /// <summary>
    /// Содержит константы типов специфических клеймов.
    /// </summary>
    public struct ClaimTypes
    {
        public const string USER_ID_CLAIM_NAME = "user_id";
    }
}
