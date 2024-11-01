﻿using BookHub.Models.Books;

using System.Collections.Generic;

namespace BookHub.Models.CRUDS.Requests;

/// <summary>
/// Параметры запроса по обновлению параметров книги.
/// </summary>
public abstract class UpdateBookParamsBase : BookParamsWithIdBase
{
    protected UpdateBookParamsBase(Id<Book> bookId) : base(bookId)
    {
    }
}