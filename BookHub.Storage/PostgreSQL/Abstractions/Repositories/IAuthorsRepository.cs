using BookHub.Models;
using BookHub.Models.Users;
using BookHub.Storage.PostgreSQL.Models;

using DomainBook = BookHub.Models.Books.Book;

namespace BookHub.Storage.PostgreSQL.Abstractions.Repositories;

/// <summary>
/// Описывает репозиторий авторов.
/// </summary>
public interface IAuthorsRepository
{
    public Task<Id<Author>> AddAuthorAsync(
        Name<Author> name,
        CancellationToken token);

    public Task<Name<Author>> GetAuthorNameAsync(
        Id<Author> id,
        CancellationToken token);

    public Task<IReadOnlyCollection<DomainBook>> GetWrittenBooksAsync(
        Id<Author> id,
        CancellationToken token);
}