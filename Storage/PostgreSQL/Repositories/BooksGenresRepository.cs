using BookHub.API.Abstractions.Storage.Repositories;
using BookHub.API.Models.Books.Repository;
using BookHub.API.Storage.PostgreSQL.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace BookHub.API.Storage.PostgreSQL.Repositories;

/// <summary>
/// Представляет репозиторий жанров книг.
/// </summary>
public sealed class BooksGenresRepository :
    RepositoryBase,
    IBooksGenresRepository
{
    public BooksGenresRepository(IRepositoryContext context)
        : base(context)
    {
    }

    public async Task AddNewGenreAsync(
        BookGenre bookGenre,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookGenre);

        var existedBookGenre = await Context.Genres
            .AsNoTracking()
            .SingleOrDefaultAsync(x => bookGenre.Value == x.Value, token);

        if (existedBookGenre is not null)
        {
            throw new InvalidOperationException(
                $"Book genre '{bookGenre.Value}' is already exists.");
        }

        _ = Context.Genres.Add(new() { Value = bookGenre.Value });
    }

    public async Task<IReadOnlyCollection<BookGenre>> GetAllGenresAsync(
        CancellationToken token)
    {
        var storageGenres = await Context.Genres.AsNoTracking().ToListAsync(token);

        return storageGenres.Select(x => new BookGenre(x.Value)).ToList();
    }

    public async Task RemoveGenreAsync(
        BookGenre bookGenre,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(bookGenre);

        var existedBookGenre =
            await Context.Genres.AsNoTracking()
                .SingleOrDefaultAsync(
                    x => bookGenre.Value == x.Value,
                    cancellationToken)
                ?? throw new InvalidOperationException(
                    $"Book genre '{bookGenre.Value}' is not already exists.");

        _ = Context.Genres.Remove(existedBookGenre);
    }
}