using BookHub.Abstractions.Storage.Repositories;
using BookHub.Models.Books;
using BookHub.Storage.PostgreSQL.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace BookHub.Storage.PostgreSQL.Repositories;

/// <summary>
/// Представляет репозиторий жанров книг.
/// </summary>
public sealed class BooksGenresRepository : RepositoryBase, IBooksGenresRepository
{
    public BooksGenresRepository(IRepositoryContext context) 
        : base(context)
    {
    }

    public async Task AddNewGenreAsync(BookGenre bookGenre, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookGenre);

        var existedBookGenre =
            await Context.Genres.AsNoTracking()
                .SingleOrDefaultAsync(x => x.Value == bookGenre.Value);

        if (existedBookGenre is not null)
        {
            throw new InvalidOperationException(
                $"Book genre '{bookGenre.Value}' is already exists.");
        }

        _ = Context.Genres.Add(new() { Value = bookGenre.Value });
    }

    public async Task<IReadOnlyCollection<BookGenre>> GetAllGenresAsync(CancellationToken token)
    {
        var storageGenres = await Context.Genres.ToListAsync(token);

        return storageGenres.Select(x => new BookGenre(x.Value)).ToList();
    }

    public async Task RemoveGenreAsync(BookGenre bookGenre, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(bookGenre);

        var existedBookGenre =
            await Context.Genres.AsNoTracking()
                .SingleOrDefaultAsync(x => x.Value == bookGenre.Value)
                ?? throw new InvalidOperationException(
                    $"Book genre '{bookGenre.Value}' is not already exists.");

        _ = Context.Genres.Remove(existedBookGenre);
    }
}