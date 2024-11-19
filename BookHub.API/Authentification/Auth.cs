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

    /// <summary>
    /// Политики авторизации.
    /// </summary>
    public struct Policies
    {
        /// <summary>
        /// Политика доступа к порталу по умолчанию.
        /// </summary>
        /// <remarks>
        /// Требует наличия зарегистрированного профиля на портале.
        /// </remarks>
        public const string DEFAULT_POLICY = "default";

        /// <summary>
        /// Политика, разрешающая автоматическую регистрацию, если аккаунта пользователя не существует.
        /// </summary>
        public const string ALLOW_REGISTER_POLICY = "allow-register";
    }
}
