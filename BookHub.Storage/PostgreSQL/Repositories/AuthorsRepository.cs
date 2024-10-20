using BookHub.Models;
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

    public async Task<Id<Author>> AddAuthorAsync(Name<Author> name)
    {
        ArgumentNullException.ThrowIfNull(name);

        var author = new Author
        {
            Name = name.Value
        };

        await Context.Authors.AddAsync(author);

        return new(author.Id);
    }

    public async Task<Name<Author>> GetAuthorNameAsync(Id<Author> id)
    {
        ArgumentNullException.ThrowIfNull(id);

        try
        {
            return new((await Context.Authors
                .SingleAsync(x => x.Id == id.Value)).Name);
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException($"No such author with id {id.Value}.");
        }
    }

    public async Task<IReadOnlyCollection<DomainBook>> GetWrittenBooks(
        Id<Author> id)
    {
        ArgumentNullException.ThrowIfNull(id);

        var storageBooks = 
            await Context.Books
                .Where(x => x.AuthorId == id.Value)
                .ToListAsync();

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
                new(x.BookText),
                x.BookStatus))
            .ToList();
    }
}