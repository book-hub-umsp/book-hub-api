using System;
using System.Collections.Generic;
using System.Linq;

using BookHub.Models.Books;
using BookHub.Models.Users;

namespace BookHub.Models.CRUDS.Requests;

/// <summary>
/// Параметры запроса по обновлению ключевых слов книги.
/// </summary>
public sealed class UpdateKeyWordsParams : UpdateBookParamsBase
{
    public IReadOnlySet<KeyWord> UpdatedKeyWords { get; }

    public UpdateKeyWordsParams(
        Id<Book> bookId,
        Id<User> authorId,
        IReadOnlyCollection<KeyWord> updatedKeyWords)
        : base(bookId, authorId)
    {
        ArgumentNullException.ThrowIfNull(updatedKeyWords);

        UpdatedKeyWords = updatedKeyWords.ToHashSet();
    }
}