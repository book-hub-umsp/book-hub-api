using BookHub.Models.Books;

namespace BookHub.Storage.PostgreSQL.Models;

/// <summary>
/// Книга.
/// </summary>
public sealed class Book
{
    public long Id { get; set; }

    public required string Caption { get; set; }

    public required long AuthorId { get; set; }

    public required BookGenre BookGenre { get; set; }

    public required string BookBriefDescription { get; set; }

    public required string BookText { get; set; }

    public required BookStatus BookStatus { get; set; }

    public required DateTimeOffset CreationDate { get; set; }

    public required DateTimeOffset LastEditDate { get; set; }

    public HashSet<FavouriteLink> LikedUsersLinks { get; set; } = null!;

    public HashSet<KeyWordLink> KeywordsLinks { get; set; } = null!;
}