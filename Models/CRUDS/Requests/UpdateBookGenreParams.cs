using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Models.CRUDS.Requests;

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