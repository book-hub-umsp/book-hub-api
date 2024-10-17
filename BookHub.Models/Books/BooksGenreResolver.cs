using System;
using System.Collections.Frozen;
using System.Collections.Generic;

namespace BookHub.Models.Books;

/// <summary>
/// Помощник по разрешению жанра книги.
/// </summary>
/// <remarks>
/// Инфу брал здесь https://blog.selfpub.ru/genresofliterature.
/// </remarks>
public static class BooksGenreResolver
{
    public static BookGenreGroup GetGenreGroup(BookGenre bookGenre)
        => bookGenre switch
        {
            _ when IsEpicGenre(bookGenre) => BookGenreGroup.EpicGroup,

            _ when IsLyricalGenre(bookGenre) => BookGenreGroup.LyricalGroup,

            _ when IsDramaGenre(bookGenre) => BookGenreGroup.DramaGroup,

            _ when IsLyricalAndEpicGenre(bookGenre) => BookGenreGroup.EpicAndLyricalGroup,

            _ => throw new InvalidOperationException($"No matching group for {bookGenre}")
        };

    public static Name<BookGenre> GetRussianResolver(BookGenre bookGenre)
        => _russianResolvers.TryGetValue(bookGenre, out var resolver)
        ? new(_russianResolvers[bookGenre])
        : throw new InvalidOperationException($"No matching russian resolver for {bookGenre}");

    public static bool IsEpicGenre(BookGenre bookGenre) => _epicGenres.Contains(bookGenre);

    public static bool IsLyricalGenre(BookGenre bookGenre) => _lyricalAndEpicGenres.Contains(bookGenre);

    public static bool IsDramaGenre(BookGenre bookGenre) => _dramaGenres.Contains(bookGenre);

    public static bool IsLyricalAndEpicGenre(BookGenre bookGenre) => _lyricalAndEpicGenres.Contains(bookGenre);

    private static readonly FrozenSet<BookGenre> _epicGenres = new HashSet<BookGenre>()
    {
        BookGenre.EpicNovel,
        BookGenre.Novel,
        BookGenre.Narrativa,
        BookGenre.Story
    }.ToFrozenSet();

    private static readonly FrozenSet<BookGenre> _lyricalGenres = new HashSet<BookGenre>()
    {
        BookGenre.Parable,
        BookGenre.LyricalPoem,
        BookGenre.Elegy,
        BookGenre.Epistle,
        BookGenre.Epigram,
        BookGenre.Ode,
        BookGenre.Sonnet
    }.ToFrozenSet();

    private static readonly FrozenSet<BookGenre> _dramaGenres = new HashSet<BookGenre>()
    {
        BookGenre.Comedy,
        BookGenre.Tradegy,
        BookGenre.Drama
    }.ToFrozenSet();

    private static readonly FrozenSet<BookGenre> _lyricalAndEpicGenres = new HashSet<BookGenre>()
    {
        BookGenre.Poem,
        BookGenre.Ballad
    }.ToFrozenSet();

    private static readonly FrozenDictionary<BookGenre, string> _russianResolvers =
        new Dictionary<BookGenre, string>()
        {
            { BookGenre.EpicNovel, "Роман-эпопея" },
            { BookGenre.Novel, "Роман" },
            { BookGenre.Narrativa, "Повесть" },
            { BookGenre.Story, "Рассказ"},
            { BookGenre.Parable, "Притча" },
            { BookGenre.LyricalPoem, "Лирическое стихотворение" },
            { BookGenre.Elegy, "Элегия" },
            { BookGenre.Epistle, "Послание" },
            { BookGenre.Epigram, "Эпиграм" },
            { BookGenre.Ode, "Ода" },
            { BookGenre.Sonnet, "Соннет" },
            { BookGenre.Comedy, "Комедия" },
            { BookGenre.Tradegy, "Трагедия" },
            { BookGenre.Drama, "Драма" },
            { BookGenre.Poem, "Поэма" },
            { BookGenre.Ballad, "Баллада" }
        }.ToFrozenDictionary();
}