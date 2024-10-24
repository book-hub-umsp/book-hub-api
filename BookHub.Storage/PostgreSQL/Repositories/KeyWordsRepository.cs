using BookHub.Models;
using BookHub.Models.Users;
using BookHub.Storage.PostgreSQL.Abstractions;
using BookHub.Storage.PostgreSQL.Abstractions.Repositories;
using BookHub.Storage.PostgreSQL.Models;
using Microsoft.EntityFrameworkCore;

using DomainKeyWord = BookHub.Models.Books.KeyWord;

namespace BookHub.Storage.PostgreSQL.Repositories;

/// <summary>
/// Описывает репозиторий для ключевых слов.
/// </summary>
public sealed class KeyWordsRepository :
    RepositoryBase,
    IKeyWordsRepository
{
    public KeyWordsRepository(IRepositoryContext context) 
        : base(context) {}

    public async Task<Id<DomainKeyWord>> AddKeyWordAsync(
        Name<DomainKeyWord> content,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(content);

        var keyWord = new KeyWord()
        {
            Content = content.Value
        };

        Context.KeyWords.Add(keyWord);

        await Context.SaveChangesAsync(token);

        return new(keyWord.Id);
    }

    public async Task<Name<DomainKeyWord>> GetKeyWordAsync(
        Id<DomainKeyWord> id, 
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(id);

        var storageKeyWord = await Context.KeyWords
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id.Value, token);

        if (storageKeyWord is null)
        {
            throw new InvalidOperationException(
                $"No such key word with id {id.Value}.");
        }

        return new(storageKeyWord.Content);
    }

    public async Task UpdateKeyWordAsync(
        DomainKeyWord newKeyWord, 
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(newKeyWord);

        var storageKeyWord =
            await Context.KeyWords.SingleOrDefaultAsync(
                x => x.Id == newKeyWord.Id.Value,
                token);

        if (storageKeyWord is null)
        {
            throw new InvalidOperationException(
                $"No such key word with id {newKeyWord.Id.Value}.");
        }

        storageKeyWord.Content = newKeyWord.Content.Value;
    }

    public async Task DeleteKeyWordAsync(
        Id<DomainKeyWord> id, 
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(id);

        var storageKeyWord =
            await Context.KeyWords.SingleAsync(
                x => x.Id == id.Value,
                token);

        if (storageKeyWord is null)
        {
            throw new InvalidOperationException(
                $"No such key word with id {id.Value}.");
        }

        _ = Context.KeyWords.Remove(storageKeyWord);
    }
}