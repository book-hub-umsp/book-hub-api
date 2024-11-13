using System;

using BookHub.Models.Account;
using BookHub.Models.Books.Repository;

namespace BookHub.Models.CRUDS.Requests;

/// <summary>
/// Параметры запроса по обновлению параметров книги.
/// </summary>
public abstract class UpdateBookParamsBase : BookParamsWithIdBase
{
    public Id<User> AuthorId { get; }

    protected UpdateBookParamsBase(Id<Book> bookId, Id<User> authorId)
        : base(bookId)
    {
        AuthorId = authorId ?? throw new ArgumentNullException(nameof(authorId));
    }
}