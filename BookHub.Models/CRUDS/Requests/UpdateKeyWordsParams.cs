using System;
using System.Collections.Generic;
using System.Linq;

using BookHub.Models.Books;

namespace BookHub.Models.CRUDS.Requests;

/// <summary>
/// Параметры запроса по обновлению ключевых слов книги.
/// </summary>
public sealed class UpdateKeyWordsParams : UpdateBookParamsBase
{
    public IReadOnlySet<KeyWord> UpdatedKeyWords { get; }

    public UpdateKeyWordsParams(
        Id<Book> bookId,
        IReadOnlyCollection<KeyWord> updatedKeyWords)
        : base(bookId)
    {
        ArgumentNullException.ThrowIfNull(updatedKeyWords);

        UpdatedKeyWords = updatedKeyWords.ToHashSet();
    }
}