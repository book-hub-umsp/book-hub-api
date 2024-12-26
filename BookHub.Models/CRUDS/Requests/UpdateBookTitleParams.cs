using System;

using BookHub.Models.Books.Repository;

namespace BookHub.Models.CRUDS.Requests;

/// <summary>
/// Параметры запроса по обновлению заголовка книги.
/// </summary>
public sealed class UpdateBookTitleParams : UpdateBookParamsBase
{
    public Name<Book> NewTitle { get; }

    public UpdateBookTitleParams(
        Id<Book> bookId,
        Name<Book> newTitle)
        : base(bookId)
    {
        NewTitle = newTitle ?? throw new ArgumentNullException(nameof(newTitle));
    }
}
