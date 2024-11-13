using System;
using System.Collections.Generic;

namespace BookHub.Models.Books.Repository;

/// <summary>
/// Описание книги.
/// </summary>
public sealed class BookDescription : IEquatable<BookDescription>
{
    public BookGenre Genre { get; private set; }

    public Name<Book> Title { get; private set; }

    public BookAnnotation BookAnnotation { get; private set; }

    public HashSet<KeyWord> KeyWords { get; private set; } = [];

    public BookDescription(
        BookGenre genre,
        Name<Book> title,
        BookAnnotation bookAnnotation)
    {
        Genre = genre ?? throw new ArgumentNullException(nameof(genre));
        Title = title ?? throw new ArgumentNullException(nameof(title));
        BookAnnotation = bookAnnotation
            ?? throw new ArgumentNullException(nameof(bookAnnotation));
    }

    public BookDescription(
        BookGenre genre,
        Name<Book> title,
        BookAnnotation bookAnnotation,
        HashSet<KeyWord> keyWords)
        : this(genre, title, bookAnnotation)
    {
        KeyWords = keyWords ?? throw new ArgumentNullException(nameof(keyWords));
    }

    public void ChangeGenre(BookGenre newGenre)
    {
        ArgumentNullException.ThrowIfNull(newGenre);

        Genre = newGenre;
    }

    public void ChangeTitle(Name<Book> newTitle)
    {
        ArgumentNullException.ThrowIfNull(newTitle);

        Title = newTitle;
    }

    public void ChangeBriefDescription(BookAnnotation newBookAnnotation)
    {
        ArgumentNullException.ThrowIfNull(newBookAnnotation);

        BookAnnotation = newBookAnnotation;
    }

    public void AddKeyWords(IReadOnlySet<KeyWord> keyWords)
    {
        ArgumentNullException.ThrowIfNull(keyWords);

        foreach (var keyWord in keyWords)
            _ = KeyWords.Add(keyWord);
    }

    public bool Equals(BookDescription? other) =>
        other is not null
        && other.Genre == Genre
        && other.Title == Title
        && other.BookAnnotation == BookAnnotation
        && other.KeyWords.SetEquals(KeyWords);

    public override bool Equals(object? obj) => Equals(obj as BookDescription);

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Genre,
            Title,
            BookAnnotation,
            GetHashSetCode(KeyWords));
    }

    private static int GetHashSetCode(HashSet<KeyWord> hashSet)
    {
        var hashCode = new HashCode();

        foreach (var item in hashSet)
            hashCode.Add(hashSet.Comparer.GetHashCode(item));

        return hashCode.ToHashCode();
    }
}