namespace BookHub.Storage.PostgreSQL.Models;

/// <summary>
/// Ссылки из избранного.
/// </summary>
public sealed class FavoriteLink
{
    public required long UserId { get; init; }

    public User User { get; set; } = null!;

    public required long BookId { get; init; }

    public Book Book { get; set; } = null!;
}
