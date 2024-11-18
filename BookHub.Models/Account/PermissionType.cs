namespace BookHub.Models.Account;

/// <summary>
/// Тип permission для действия пользователя.
/// </summary>
public enum PermissionType
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