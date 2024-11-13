using System;

using BookHub.Models.Account;
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
        Id<User> authorId,
        Name<Book> newTitle)
        : base(bookId, authorId)
    {
        NewTitle = newTitle ?? throw new ArgumentNullException(nameof(newTitle));
    }
}
