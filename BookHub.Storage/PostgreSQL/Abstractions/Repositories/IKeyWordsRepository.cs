using BookHub.Models;
using BookHub.Models.Users;
using DomainKeyWord = BookHub.Models.Books.KeyWord;

namespace BookHub.Storage.PostgreSQL.Abstractions.Repositories;

/// <summary>
/// Описывает репозиторий для ключевых слов.
/// </summary>
public interface IKeyWordsRepository
{
    public Task<Id<DomainKeyWord>> AddKeyWordAsync(
        Name<DomainKeyWord> content,
        CancellationToken token);

    public Task<Name<DomainKeyWord>> GetKeyWordAsync(
        Id<DomainKeyWord> id, 
        CancellationToken token);

    public Task UpdateKeyWordAsync(
        DomainKeyWord newKeyWord,
        CancellationToken token);

    public Task DeleteKeyWordAsync(
        Id<DomainKeyWord> id, 
        CancellationToken token);
}