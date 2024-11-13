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
        /// <summary>
        /// Google.
        /// </summary>
        public const string GOOGLE = "google";

        /// <summary>
        /// Yandex.
        /// </summary>
        public const string YANDEX = "yandex";
    }

    /// <summary>
    /// Содержит константы типов специфических клеймов.
    /// </summary>
    public struct ClaimTypes
    {
        /// <summary>
        /// Клейм идентификатора пользователя.
        /// </summary>
        public const string USER_ID_CLAIM_NAME = "user_id";
    }
}
