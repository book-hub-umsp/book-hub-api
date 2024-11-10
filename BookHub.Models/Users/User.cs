using System;
using System.ComponentModel;

namespace BookHub.Models.Users;

/// <summary>
/// Пользователь системы.
/// </summary>
public sealed class User
{
    public UserProfileInfo ProfileInfo { get; }

    public UserRole Role { get; }

    public UserStatus Status { get; }

    public User(
        UserProfileInfo profileInfo,
        UserRole role,
        UserStatus status)
    {
        ArgumentNullException.ThrowIfNull(profileInfo);
        ProfileInfo = profileInfo;

        if (!Enum.IsDefined(role))
        {
            throw new InvalidEnumArgumentException(
                nameof(role),
                (int)role,
                typeof(UserRole));
        }
        Role = role;

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