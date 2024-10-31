using System.Data;

using BookHub.Abstractions;
using BookHub.Abstractions.Repositories;

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

    public IBooksRepository Books { get; }

    public BooksHubContext Context { get; }

    public BooksHubUnitOfWork(
        IBooksRepository books,
        IUsersRepository users,
        BooksHubContext context)
    {
        Users = users ?? throw new ArgumentNullException(nameof(users));
        Books = books ?? throw new ArgumentNullException(nameof(books));
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