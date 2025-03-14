﻿using System.Net.Mail;

using BookHub.API.Models.Identifiers;

namespace BookHub.API.Models.Account;

/// <summary>
/// Информация о профиле пользователя.
/// </summary>
public sealed class UserProfileInfo
{
    public Id<User> Id { get; }

    public Name<User> Name { get; }

    public MailAddress Email { get; }

    public About About { get; }

    public UserProfileInfo(
        Id<User> id,
        Name<User> name,
        MailAddress email,
        About about)
    {
        ArgumentNullException.ThrowIfNull(id);
        Id = id;

        ArgumentNullException.ThrowIfNull(name);
        Name = name;

        ArgumentNullException.ThrowIfNull(email);
        Email = email;

        ArgumentNullException.ThrowIfNull(about);
        About = about;
    }
}
