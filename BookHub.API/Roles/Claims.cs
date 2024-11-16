using BookHub.Models.Account;

namespace BookHub.API.Roles;

/// <summary>
/// Набор строчных именованных клэймов.
/// </summary>
public record struct Claims
{
    public const string NONE = "None";

    public const string MODERATE_COMMENTS = "ModerateComments";

    public const string MODERATE_REVIEWS = "ModerateReviews";

    public const string CREATE_TOPICS = "CreateTopics";

    public const string CHANGE_BOOK_VISIBILITY = "ChangeBookVisibility";

    public const string MANAGE_USER_ACTIONS = "ManageUserActions";

    public const string MANAGE_USER_ACCOUNTS = "ManageUserAccounts";

    public const string ADD_NEW_ROLE = "AddNewRole";

    public const string CHANGE_USER_ROLE = "ChangeUserRole";

    public const string CHANGE_ROLE_CLAIMS = "ChangeRoleClaims";

    /// <summary>
    /// Возвращает строчное представление для <see cref="ClaimType"/>.
    /// </summary>
    /// <param name="claimType">
    /// Клэйм.
    /// </param>
    public static string GetStringClaim(ClaimType claimType) => _claimsMapping[claimType];

    private static readonly Dictionary<ClaimType, string> _claimsMapping =
        new()
        {
            { ClaimType.None, NONE },
            { ClaimType.ModerateComments, MODERATE_COMMENTS },
            { ClaimType.ModerateReviews, MODERATE_REVIEWS },
            { ClaimType.CreateTopics, CREATE_TOPICS },
            { ClaimType.ChangeBookVisibility, CHANGE_BOOK_VISIBILITY },
            { ClaimType.ManageUserActions, MANAGE_USER_ACTIONS },
            { ClaimType.ManageUserAccounts, MANAGE_USER_ACCOUNTS },
            { ClaimType.AddNewRole, ADD_NEW_ROLE },
            { ClaimType.ChangeUserRole, CHANGE_USER_ROLE },
            { ClaimType.ChangeRoleClaims, CHANGE_ROLE_CLAIMS }
        };
}