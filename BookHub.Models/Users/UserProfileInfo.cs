using System;

namespace BookHub.Models.Users;

/// <summary>
/// Информация о профиле пользователя.
/// </summary>
public sealed class UserProfileInfo
{
    public Name<User> Name { get; }

    public Email Email { get; }

    public About About { get; }

    public UserProfileInfo(
        Name<User> name,
        Email email,
        About about)
    {
        ArgumentNullException.ThrowIfNull(name);
        Name = name;

        ArgumentNullException.ThrowIfNull(email);
        Email = email;

        ArgumentNullException.ThrowIfNull(about);
        About = about;
    }
}
