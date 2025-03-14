using BookHub.API.Models.Account;

namespace BookHub.API.Storage.PostgreSQL.Models;

/// <summary>
/// Модель роли в хранилище.
/// </summary>
public sealed class Role : IKeyable
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public Permission[] Permissions { get; set; } = null!;

    public ICollection<User> Users { get; set; } = null!;
}