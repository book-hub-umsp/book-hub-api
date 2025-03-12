using BookHub.API.Models.Books.Repository;

namespace BookHub.API.Storage.PostgreSQL.Models;

/// <summary>
/// Книга.
/// </summary>
public class Book : IKeyable
{
    public long Id { get; set; }

    public required string Title { get; set; }

    public User Author { get; set; } = null!;

    public required long AuthorId { get; set; }

    public long BookGenreId { get; set; }

    public BookGenre BookGenre { get; set; } = null!;

    public required string BookAnnotation { get; set; }

    public required BookStatus BookStatus { get; set; }

    public required DateTimeOffset CreationDate { get; set; }

    public required DateTimeOffset LastEditDate { get; set; }

    public ICollection<Partition> Partitions { get; set; } = null!;

    public ICollection<FavoriteLink> UsersFavoritesLinks { get; set; } = null!;

    public ICollection<KeywordLink> KeywordLinks { get; set; } = null!;
}