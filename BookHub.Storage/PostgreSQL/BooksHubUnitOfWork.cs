using System.Data;

using BookHub.Abstractions.Storage;
using BookHub.Abstractions.Storage.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookHub.Storage.PostgreSQL;

/// <summary>
/// Представляет единицу работы с хранилищем книг
/// </summary>
public sealed class BooksHubUnitOfWork :
    IBooksHubUnitOfWork,
    IAsyncDisposable
{
    public IUsersRepository Users { get; }

    public IRolesRepository UserRoles { get; }

    public IBooksRepository Books { get; }

    public IChaptersRepository Chapters { get; }

    public IBooksGenresRepository BooksGenres { get; }

    public IFavoriteLinkRepository FavoriteLinks { get; }

    public BooksHubContext Context { get; }

    public BooksHubUnitOfWork(
        IBooksRepository books,
        IChaptersRepository chapters,
        IUsersRepository users,
        IRolesRepository userRoles,
        IBooksGenresRepository booksGenres,
        IFavoriteLinkRepository favoriteLinks,
        BooksHubContext context)
    {
        Users = users ?? throw new ArgumentNullException(nameof(users));
        Books = books ?? throw new ArgumentNullException(nameof(books));
        Chapters = chapters ?? throw new ArgumentNullException(nameof(chapters));
        UserRoles = userRoles ?? throw new ArgumentNullException(nameof(userRoles));
        BooksGenres = booksGenres ?? throw new ArgumentNullException(nameof(booksGenres));
        FavoriteLinks = favoriteLinks ?? throw new ArgumentNullException(nameof(favoriteLinks));
        Context = context ?? throw new ArgumentNullException(nameof(context));
        _dbContextTransaction =
            Context.Database.BeginTransaction(IsolationLevel.ReadCommitted);
    }

    public async Task SaveChangesAsync(CancellationToken token)
    {
        await Context.SaveChangesAsync(token);

        await _dbContextTransaction.CommitAsync(token);
    }

    public async ValueTask DisposeAsync() => await _dbContextTransaction.DisposeAsync();

    private readonly IDbContextTransaction _dbContextTransaction;
}