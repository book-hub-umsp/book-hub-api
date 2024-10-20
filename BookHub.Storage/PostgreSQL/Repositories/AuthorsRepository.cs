using BookHub.Models;
using BookHub.Storage.PostgreSQL.Abstractions;
using BookHub.Storage.PostgreSQL.Abstractions.Repositories;
using BookHub.Storage.PostgreSQL.Models;

using DomainBook = BookHub.Models.Books.Book;

namespace BookHub.Storage.PostgreSQL.Repositories;

/// <summary>
/// Описывает репозиторией авторов.
/// </summary>
public sealed class AuthorsRepository :
    RepositoriesBase,
    IAuthorsRepository
{
    public AuthorsRepository(IRepositoryContext context) 
        : base(context)
    {
    }

    public Task AddAuthorAsync(Id<Author> id, Name<Author> name)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyCollection<DomainBook>> GetWrittenBooks(Id<Author> id)
    {
        throw new NotImplementedException();
    }
}
