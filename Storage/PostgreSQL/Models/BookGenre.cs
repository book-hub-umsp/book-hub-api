﻿namespace BookHub.API.Storage.PostgreSQL.Models;

/// <summary>
/// Жанр книги.
/// </summary>
public sealed class BookGenre : IKeyable
{
    public long Id { get; set; }

    public required string Value { get; init; }

    public ICollection<Book> Books { get; set; } = null!;
}