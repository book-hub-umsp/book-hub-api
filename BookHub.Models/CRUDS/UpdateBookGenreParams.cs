using System;

using BookHub.Models.Books;

namespace BookHub.Models.CRUDS;

/// <summary>
/// Параметры запроса по обновлению жанра книги.
/// </summary>
public sealed class UpdateBookGenreParams : UpdateBookParamsBase
{
    public BookGenre NewGenre { get; }

    public UpdateBookGenreParams(
        Id<Book> bookId, 
        BookGenre newGenre)
        : base(bookId)
    {
        NewGenre = newGenre ?? throw new ArgumentNullException(nameof(newGenre));
    }
}