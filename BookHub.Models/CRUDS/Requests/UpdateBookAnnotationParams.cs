using System;

using BookHub.Models.Books;

namespace BookHub.Models.CRUDS.Requests;

/// <summary>
/// Параметры запроса по обновлению аннотации книги.
/// </summary>
public sealed class UpdateBookAnnotationParams : UpdateBookParamsBase
{
    public BookAnnotation NewBookAnnotation { get; }

    public UpdateBookAnnotationParams(
        Id<Book> bookId,
        BookAnnotation newBookAnnotation)
        : base(bookId)
    {
        NewBookAnnotation = newBookAnnotation
            ?? throw new ArgumentNullException(nameof(newBookAnnotation));
    }
}
