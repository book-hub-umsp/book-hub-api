using BookHub.Models;
using BookHub.Storage.PostgreSQL.Abstractions;
using BookHub.Storage.PostgreSQL.Abstractions.Repositories;

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

    public Task AddKeyWordAsync(DomainKeyWord keyWord)
    {
        throw new NotImplementedException();
    }

    public Task GetKeyWord(Id<DomainKeyWord> id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateKeyWord(DomainKeyWord newKeyWord)
    {
        throw new NotImplementedException();
    }

    public Task DeleteKeyWordAsync(Id<DomainKeyWord> id)
    {
        throw new NotImplementedException();
    }
}
