using BookHub.Models;
using BookHub.Storage.PostgreSQL.Models;

using DomainBook = BookHub.Models.Books.Book;

namespace BookHub.Storage.PostgreSQL.Abstractions.Repositories;

/// <summary>
/// Описывает репозиторий авторов.
/// </summary>
public interface IAuthorsRepository
{
    public Task AddAuthorAsync(Id<Author> id, Name<Author> name);

    public Task<IReadOnlyCollection<DomainBook>> GetWrittenBooks(Id<Author> id);
}