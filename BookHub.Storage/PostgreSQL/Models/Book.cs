﻿using BookHub.Models.Books.Repository;

namespace BookHub.Storage.PostgreSQL.Models;

/// <summary>
/// Книга.
/// </summary>
public class Book : IKeyable
{
    public long Id { get; set; }

    public required string Title { get; set; }

    public User Author { get; set; } = null!;

    public required long AuthorId { get; set; }

    public required long BookGenreId { get; set; }

    public BookGenre BookGenre { get; set; } = null!;

    public required string BookAnnotation { get; set; }

    public required BookStatus BookStatus { get; set; }

    public required DateTimeOffset CreationDate { get; set; }

    public required DateTimeOffset LastEditDate { get; set; }

    public ICollection<Chapter> Chapters { get; set; } = null!;

    public virtual ICollection<FavoriteLink> UsersFavoritesLinks { get; set; } = null!;

    public virtual ICollection<KeywordLink> KeywordLinks { get; set; } = null!;
}