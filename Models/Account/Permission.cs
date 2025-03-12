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

    ModerateComments,

    ModerateReviews,

    CreateTopics,

    ChangeBookVisibility,

    ManageUserActions,

    ManageUserAccounts,

    ChangeUserRole,

    ChangeRolePermissions,
}