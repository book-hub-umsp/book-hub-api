using System.Net.Mail;

namespace BookHub.API.Models.Account;

/// <summary>
/// Регистрирующийся в системе пользователь.
/// </summary>
public sealed class RegisteringUser
{
    public Name<User> Name { get; }

    public MailAddress Email { get; }

    public RegisteringUser(MailAddress email)
    {
        ArgumentNullException.ThrowIfNull(email);
        Email = email;

        Name = new(email.User);
    }
}
