using BookHub.Models;
using BookHub.Storage.PostgreSQL.Models;

using DomainBook = BookHub.Models.Books.Book;

namespace BookHub.Storage.PostgreSQL.Abstractions.Repositories;

/// <summary>
/// Описывает репозиторий авторов.
/// </summary>
public interface IAuthorsRepository
{
    public Task<Id<Author>> AddAuthorAsync(Name<Author> name);

    public Task<Name<Author>> GetAuthorNameAsync(Id<Author> id);

    public Task<IReadOnlyCollection<DomainBook>> GetWrittenBooks(Id<Author> id);
}