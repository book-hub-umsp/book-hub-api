using BookHub.Models;

using DomainKeyWord = BookHub.Models.Books.KeyWord;

namespace BookHub.Storage.PostgreSQL.Abstractions.Repositories;

/// <summary>
/// Описывает репозиторий для ключевых слов.
/// </summary>
public interface IKeyWordsRepository
{
    public Task<Id<DomainKeyWord>> AddKeyWordAsync(
        Name<DomainKeyWord> content);

    public Task<Name<DomainKeyWord>> GetKeyWord(Id<DomainKeyWord> id);

    public Task UpdateKeyWord(DomainKeyWord newKeyWord);

    public Task DeleteKeyWordAsync(Id<DomainKeyWord> id);
}