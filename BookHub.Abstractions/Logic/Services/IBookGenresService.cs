﻿using BookHub.Models.Books;

namespace BookHub.Abstractions.Logic.Services;

/// <summary>
/// Описывает сервис для работы с жанрами книг.
/// </summary>
public interface IBookGenresService
{
    public Task AddBookGenreAsync(BookGenre bookGenre, CancellationToken token);

    public Task<IReadOnlyCollection<BookGenre>> GetBooksGenresAsync(CancellationToken token);

    public Task RemoveBookGenreAsync(BookGenre bookGenre, CancellationToken token);
}