using System;

using BookHub.Models.Books;
using BookHub.Models.Users;

namespace BookHub.Models.CRUDS.Requests;

/// <summary>
/// Параметры запроса по обновлению аннотации книги.
/// </summary>
public sealed class UpdateBookAnnotationParams : UpdateBookParamsBase
{
    public BookAnnotation NewBookAnnotation { get; }

    public UpdateBookAnnotationParams(
        Id<Book> bookId,
        Id<User> authorId,
        BookAnnotation newBookAnnotation)
        : base(bookId, authorId)
    {
        NewBookAnnotation = newBookAnnotation
            ?? throw new ArgumentNullException(nameof(newBookAnnotation));
    }
}
