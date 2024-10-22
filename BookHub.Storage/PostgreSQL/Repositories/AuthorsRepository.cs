using BookHub.Models;
using BookHub.Models.Users;
using BookHub.Storage.PostgreSQL.Abstractions;
using BookHub.Storage.PostgreSQL.Abstractions.Repositories;
using BookHub.Storage.PostgreSQL.Models;
using Microsoft.EntityFrameworkCore;

using DomainBook = BookHub.Models.Books.Book;
using DomainKeyWord = BookHub.Models.Books.KeyWord;

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

    public async Task<Id<Author>> AddAuthorAsync(
        Name<Author> name,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(name);

        var author = new Author
        {
            Name = name.Value
        };

        Context.Authors.Add(author);

        await Context.SaveChangesAsync(token);

        return new(author.Id);
    }

    public async Task<Name<Author>> GetAuthorNameAsync(
        Id<Author> id,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(id);

        try
        {
            return new((await Context.Authors
                .AsNoTracking()
                .SingleAsync(x => x.Id == id.Value, token)).Name);
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException($"No such author with id {id.Value}.");
        }
    }

    public async Task<IReadOnlyCollection<DomainBook>> GetWrittenBooksAsync(
        Id<Author> id,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(id);

        var storageBooks =
            await Context.Books
                .Where(x => x.AuthorId == id.Value)
                .AsNoTracking()
                .ToListAsync(token);

        return storageBooks
            .Select(x => new DomainBook(
                new(x.Id),
                new(x.AuthorId),
                new(
                    x.BookGenre,
                    new(x.Title),
                    new(x.BookAnnotation),
                    new(x.KeywordsLinks.Select(
                        x => new DomainKeyWord(
                            new(x.KeyWordId),
                            new(x.KeyWord.Content))))),
                x.BookStatus))
            .ToList();
    }
}