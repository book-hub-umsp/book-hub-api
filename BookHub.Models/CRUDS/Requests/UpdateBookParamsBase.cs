using BookHub.Models.Books;
using BookHub.Models.Users;

using System;
using System.Collections.Generic;

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