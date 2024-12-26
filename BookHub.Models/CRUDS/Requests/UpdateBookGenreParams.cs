using System;

using BookHub.Models.Account;
using BookHub.Models.Books.Repository;

namespace BookHub.Models.CRUDS.Requests;

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