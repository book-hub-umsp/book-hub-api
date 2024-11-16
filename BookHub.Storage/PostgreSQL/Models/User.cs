using BookHub.Models.Account;

namespace BookHub.Storage.PostgreSQL.Models;

/// <summary>
/// Пользователь.
/// </summary>
public class User : IKeyable
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public UserStatus Status { get; set; }

    public string About { get; set; } = null!;

    public ICollection<Book> WrittenBooks { get; set; } = null!;

    public ICollection<FavoriteLink> FavoriteBooksLinks { get; set; } = null!;
}
