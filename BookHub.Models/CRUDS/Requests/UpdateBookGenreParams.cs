using System;

using BookHub.Models.Books;
using BookHub.Models.Users;

namespace BookHub.Models.CRUDS.Requests;

/// <summary>
/// Параметры запроса по обновлению жанра книги.
/// </summary>
public sealed class UpdateBookGenreParams : UpdateBookParamsBase
{
    public BookGenre NewGenre { get; }

    public UpdateBookGenreParams(
        Id<Book> bookId,
        Id<User> authorId,
        BookGenre newGenre)
        : base(bookId, authorId)
    {
        NewGenre = newGenre ?? throw new ArgumentNullException(nameof(newGenre));
    }
}