using BookHub.Models;
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
    RepositoriesBase,
    IKeyWordsRepository
{
    public KeyWordsRepository(IRepositoryContext context) 
        : base(context)
    {
    }

    public async Task<Id<DomainKeyWord>> AddKeyWordAsync(
        Name<DomainKeyWord> content)
    {
        ArgumentNullException.ThrowIfNull(content);

        var keyWord = new KeyWord()
        {
            Content = content.Value
        };

        await Context.KeyWords.AddAsync(keyWord);

        return new(keyWord.Id);
    }

    public async Task<Name<DomainKeyWord>> GetKeyWord(Id<DomainKeyWord> id)
    {
        ArgumentNullException.ThrowIfNull(id);

        try
        {
            return new((await Context.KeyWords
                .SingleAsync(x => x.Id == id.Value))!.Content);
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException(
                $"No such key word with id {id.Value}.");
        }
    }

    public async Task UpdateKeyWord(DomainKeyWord newKeyWord)
    {
        ArgumentNullException.ThrowIfNull(newKeyWord);

        try
        {
            var storageKeyWord =
                await Context.KeyWords.SingleAsync(x => x.Id == newKeyWord.Id.Value);

            storageKeyWord!.Content = newKeyWord.Content.Value;
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException(
                $"No such key word with id {newKeyWord.Id.Value}.");
        }
    }

    public async Task DeleteKeyWordAsync(Id<DomainKeyWord> id)
    {
        ArgumentNullException.ThrowIfNull(id);

        try
        {
            var storageKeyWord =
                await Context.KeyWords.SingleAsync(x => x.Id == id.Value);

            _ = Context.KeyWords.Remove(storageKeyWord);
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException(
                $"No such key word with id {id.Value}.");
        }
    }
}