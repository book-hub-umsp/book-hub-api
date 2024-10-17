using BookHub.Models;

namespace BookHub.Storage.PostgreSQL.Models;

/// <summary>
/// Ссылки из избранного.
/// </summary>
public sealed class FavouriteLink
{
    public required long AuthorId { get; init; }

    public Author Author { get; set; }

    public required long BookId { get; init; }

    public Book Book { get; set; }
}