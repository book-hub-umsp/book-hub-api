using System;
using System.ComponentModel;

namespace BookHub.Models.Users;

/// <summary>
/// Пользователь системы.
/// </summary>
public sealed class User
{
    public UserProfileInfo ProfileInfo { get; }

    public UserStatus Status { get; }

    public UserPermission Permission { get; }

    public User(
        UserProfileInfo profileInfo,
        UserStatus status,
        UserPermission premission)
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

        if (!Enum.IsDefined(premission))
        {
            throw new InvalidEnumArgumentException(
                nameof(premission),
                (int)premission,
                typeof(UserPermission));
        }
        Permission = premission;
    }
}
