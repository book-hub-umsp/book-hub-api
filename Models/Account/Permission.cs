namespace BookHub.API.Models.Account;

/// <summary>
/// Разрешение для действия пользователя.
/// </summary>
public enum Permission
{
    /// <summary>
    /// Дефолтное значение, заглушка.
    /// </summary>
    None = 0,

    ModerateAccounts,
}