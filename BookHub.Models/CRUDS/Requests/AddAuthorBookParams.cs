using System;
using System.Collections.Generic;

using BookHub.Models.Account;
using BookHub.Models.Books.Repository;

namespace BookHub.Models.CRUDS.Requests;

/// <summary>
/// Параметры команды добавления новой книги с автором.
/// </summary>
public sealed class AddAuthorBookParams : AddBookParams
{
    public Id<User> AuthorId { get; }

    public AddAuthorBookParams(
        Id<User> authorId,
        BookGenre genre,
        Name<Book> title,
        BookAnnotation annotation)
        : base(genre, title, annotation)
    {
        AuthorId = authorId ?? throw new ArgumentNullException(nameof(authorId));
    }

    public AddAuthorBookParams(
        Id<User> authorId,
        BookGenre genre,
        Name<Book> title,
        BookAnnotation annotation,
        IReadOnlyCollection<KeyWord> keyWords)
        : base(genre, title, annotation, keyWords)
    {
        AuthorId = authorId ?? throw new ArgumentNullException(nameof(authorId));
    }
}