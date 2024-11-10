namespace BookHub.API.Authentification;

public struct Auth
{
    public struct AuthProviders
    {
        public const string GOOGLE = "google";

        public const string YANDEX = "yandex";
    }

    public struct ClaimTypes
    {
        public const string USER_ID_CLAIM_NAME = "user_id";
    }
}
