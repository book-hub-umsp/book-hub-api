using System;
using System.Collections.Generic;
using System.Linq;

using BookHub.Models.Books;

namespace BookHub.Models.CRUDS.Requests;

/// <summary>
/// Параметры команды добавления новой книги.
/// </summary>
public abstract class AddBookParams : BookParamsBase
{
    public BookGenre Genre { get; }

    public Name<Book> Title { get; }

    public BookAnnotation Annotation { get; }

    public IReadOnlySet<KeyWord>? Keywords { get; }

    protected AddBookParams(
        BookGenre genre,
        Name<Book> title,
        BookAnnotation annotation)
    {
        Genre = genre ?? throw new ArgumentNullException(nameof(genre));
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Annotation = annotation ?? throw new ArgumentNullException(nameof(annotation));
    }

    protected AddBookParams(
        BookGenre genre,
        Name<Book> title,
        BookAnnotation annotation,
        IReadOnlyCollection<KeyWord> keyWords)
        : this(genre, title, annotation)
    {
        ArgumentNullException.ThrowIfNull(keyWords);

        Keywords = keyWords.ToHashSet();
    }
}