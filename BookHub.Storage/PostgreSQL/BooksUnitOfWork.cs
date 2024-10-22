using BookHub.Storage.PostgreSQL.Abstractions;
using BookHub.Storage.PostgreSQL.Abstractions.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace BookHub.Storage.PostgreSQL;

/// <summary>
/// Представляет единицу работы с хранилищем книг
/// </summary>
public sealed class BooksUnitOfWork : 
    IBooksUnitOfWork, 
    IAsyncDisposable
{
    public IAuthorsRepository Authors { get; }

    public IBooksRepository Books { get; }

    public IKeyWordsRepository KeyWords { get; }

    public BooksContext Context { get; }

    public BooksUnitOfWork(
        IAuthorsRepository authors, 
        IBooksRepository books, 
        IKeyWordsRepository keyWords, 
        BooksContext context)
    {
        Authors = authors ?? throw new ArgumentNullException(nameof(authors));
        Books = books ?? throw new ArgumentNullException(nameof(books));
        KeyWords = keyWords ?? throw new ArgumentNullException(nameof(keyWords));
        Context = context ?? throw new ArgumentNullException(nameof(context));
        _dbContextTransaction = 
            Context.Database.BeginTransaction(IsolationLevel.ReadCommitted);
    }

    public async Task SaveChangesAsync(CancellationToken token)
    {
        await Context.SaveChangesAsync(token);

        await _dbContextTransaction.CommitAsync(token);
    }

    public async ValueTask DisposeAsync()
    {
        await _dbContextTransaction.DisposeAsync();
    }

    private readonly IDbContextTransaction _dbContextTransaction;
}