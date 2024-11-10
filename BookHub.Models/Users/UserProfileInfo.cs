using System;
using System.ComponentModel;
using System.Net.Mail;

namespace BookHub.Models.Users;

/// <summary>
/// Информация о профиле пользователя.
/// </summary>
public sealed class UserProfileInfo
{
    public Id<User> Id { get; }

    public Name<User> Name { get; }

    public MailAddress Email { get; }

    public About About { get; }

    public UserRole Role { get; }

    public UserProfileInfo(
        Id<User> id,
        Name<User> name,
        MailAddress email,
        About about,
        UserRole role)
    {
        ArgumentNullException.ThrowIfNull(id);
        Id = id;

        ArgumentNullException.ThrowIfNull(name);
        Name = name;

        ArgumentNullException.ThrowIfNull(email);
        Email = email;

        ArgumentNullException.ThrowIfNull(about);
        About = about;

        if (!Enum.IsDefined(role))
        {
            throw new InvalidEnumArgumentException(
                nameof(role),
                (int)role,
                typeof(UserRole));
        }
        Role = role;

    }
}