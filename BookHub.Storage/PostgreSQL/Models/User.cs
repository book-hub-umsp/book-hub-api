using BookHub.Models.Users;

namespace BookHub.Storage.PostgreSQL.Models;

/// <summary>
/// Пользователь.
/// </summary>
public class User
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public UserStatus Status { get; set; }

    public string About { get; set; } = null!;
}
