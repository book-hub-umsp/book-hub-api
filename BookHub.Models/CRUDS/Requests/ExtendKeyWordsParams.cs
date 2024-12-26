using System;
using System.Collections.Generic;
using System.Linq;

using BookHub.Models.Books.Repository;

namespace BookHub.Models.CRUDS.Requests;

/// <summary>
/// Параметры запроса по обновлению ключевых слов книги.
/// </summary>
public sealed class ExtendKeyWordsParams : UpdateBookParamsBase
{
    public IReadOnlySet<KeyWord> UpdatedKeyWords { get; }

    public ExtendKeyWordsParams(
        Id<Book> bookId,
        IReadOnlyCollection<KeyWord> updatedKeyWords)
        : base(bookId)
    {
        ArgumentNullException.ThrowIfNull(updatedKeyWords);

        UpdatedKeyWords = updatedKeyWords.ToHashSet();
    }
}