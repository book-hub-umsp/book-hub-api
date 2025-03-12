using System.ComponentModel;

namespace BookHub.API.Models.Account;

/// <summary>
/// Пользователь системы.
/// </summary>
public sealed class User
{
    public UserProfileInfo ProfileInfo { get; }

    public UserStatus Status { get; }

    public User(
        UserProfileInfo profileInfo,
        UserStatus status)
    {
        ArgumentNullException.ThrowIfNull(profileInfo);
        ProfileInfo = profileInfo;

        if (!Enum.IsDefined(status))
        {
            throw new InvalidEnumArgumentException(
                nameof(status),
                (int)status,
                typeof(UserStatus));
        }

        Status = status;
    }
}
