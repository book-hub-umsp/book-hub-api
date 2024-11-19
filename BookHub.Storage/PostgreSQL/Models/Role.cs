using BookHub.Models.Account;

namespace BookHub.Storage.PostgreSQL.Models;

/// <summary>
/// Модель роли в хранилище.
/// </summary>
public sealed class Role
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public Permission[] Permissions { get; set; } = null!;

    public ICollection<User> Users { get; set; } = null!;
}