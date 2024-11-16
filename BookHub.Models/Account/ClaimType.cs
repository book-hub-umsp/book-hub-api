namespace BookHub.Models.Account;

/// <summary>
/// Тип клэйма для действия пользователя.
/// </summary>
/// <remarks>
/// Накидал в целом примеры некоторые.
/// </remarks>
public enum ClaimType
{
    None,

    ModerateComments,

    ModerateReviews,

    CreateTopics,

    ChangeBookVisibility,

    ManageUserActions,

    ManageUserAccounts,

    ChangeUserRole,

    AddNewRole,

    ChangeRoleClaims,
}