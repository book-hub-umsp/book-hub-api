using BookHub.Models;

namespace BookHub.Storage.PostgreSQL.Models;

/// <summary>
/// Ссылки из избранного.
/// </summary>
public sealed class FavouriteLink
{
    public required Id<Author> AuthorId { get; init; }

    public required Id<Book> BookId { get; init; }
}