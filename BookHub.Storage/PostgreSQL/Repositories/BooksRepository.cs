using BookHub.Models;
using BookHub.Models.Books;
using BookHub.Storage.PostgreSQL.Abstractions;
using BookHub.Storage.PostgreSQL.Abstractions.Repositories;
using BookHub.Storage.PostgreSQL.Models;
using Microsoft.EntityFrameworkCore;

using DomainBook = BookHub.Models.Books.Book;
using DomainKeyWord = BookHub.Models.Books.KeyWord;
using StorageBook = BookHub.Storage.PostgreSQL.Models.Book;

namespace BookHub.Storage.PostgreSQL.Repositories;

/// <summary>
/// Репозиторий книг.
/// </summary>
public sealed class BooksRepository :
    RepositoriesBase, IBooksRepository
{
    public BooksRepository(IRepositoryContext context)
        : base(context)
    {
    }

    public async Task<Id<DomainBook>> AddBookAsync(DomainBook book)
    {
        ArgumentNullException.ThrowIfNull(book);

        var storageBook = new StorageBook
        {
            AuthorId = book.AuthorId.Value,
            Title = book.Description.Title.Value,
            BookGenre = book.Description.Genre,
            BookAnnotation = book.Description.BookAnnotation.Content,
            BookStatus = book.Status,
            CreationDate = book.CreationDate,
            LastEditDate = book.LastEditDate
        };

        await Context.Books.AddAsync(storageBook);

        storageBook.KeywordsLinks =
            book.Description.KeyWords
                .Select(x => new KeyWordLink()
                {
                    BookId = storageBook.Id,
                    KeyWordId = x.Id.Value
                })
                .ToHashSet();

        return new(storageBook.Id);
    }

    public async Task<DomainBook> GetBook(Id<DomainBook> id)
    {
        ArgumentNullException.ThrowIfNull(id);

        try
        {
            var storageBook =
                await Context.Books.SingleAsync(x => x.Id == id.Value);

            return new DomainBook(
                    new(storageBook.Id),
                    new(storageBook.AuthorId),
                    new(
                        storageBook.BookGenre,
                        new(storageBook.Title),
                        new(storageBook.BookAnnotation),
                        new(storageBook.KeywordsLinks.Select(
                            x => new DomainKeyWord(
                                new(x.KeyWordId), 
                                new(x.KeyWord.Content))))),
                    storageBook.BookStatus);
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException(
                $"No such book with id {id.Value}.");
        }
    }

    public async Task<bool> IsBookRelatedForCurrentAuthor(
        Id<DomainBook> bookId,
        Id<Author> authorId)
    {
        ArgumentNullException.ThrowIfNull(bookId);
        ArgumentNullException.ThrowIfNull(authorId);

        try
        {
            var storageBook = await Context.Books
                .SingleAsync(x => x.Id == bookId.Value);

            return storageBook.AuthorId == authorId.Value;
        }
        catch
        {
            throw new InvalidOperationException(
                $"No such book with id {bookId.Value}.");
        }
    }

    public async Task UpdateBookAnnotation(
        Id<DomainBook> bookId,
        BookAnnotation newBookAnnotation)
    {
        ArgumentNullException.ThrowIfNull(bookId);
        ArgumentNullException.ThrowIfNull(newBookAnnotation);

        try
        {
            var storageBook = await Context.Books
                .SingleAsync(x => x.Id == bookId.Value);

            storageBook.BookAnnotation = newBookAnnotation.Content;
        }
        catch
        {
            throw new InvalidOperationException(
                $"No such book with id {bookId.Value}.");
        }
    }

    public async Task UpdateBookDescription(
        Id<DomainBook> bookId,
        BookDescription newBookDescription)
    {
        ArgumentNullException.ThrowIfNull(bookId);
        ArgumentNullException.ThrowIfNull(newBookDescription);

        try
        {
            var storageBook = await Context.Books
                .SingleAsync(x => x.Id == bookId.Value);

            storageBook.BookGenre = newBookDescription.Genre;
            storageBook.Title = newBookDescription.Title.Value;
            storageBook.BookAnnotation = newBookDescription.BookAnnotation.Content;
            storageBook.KeywordsLinks =
                newBookDescription.KeyWords
                    .Select(x => new KeyWordLink()
                    {
                        BookId = storageBook.Id,
                        KeyWordId = x.Id.Value
                    })
                    .ToHashSet();
        }
        catch
        {
            throw new InvalidOperationException(
                $"No such book with id {bookId.Value}.");
        }
    }

    public async Task UpdateBookStatus(
        Id<DomainBook> bookId,
        BookStatus newBookStatus)
    {
        ArgumentNullException.ThrowIfNull(bookId);
        ArgumentNullException.ThrowIfNull(newBookStatus);

        try
        {
            var storageBook = await Context.Books
                .SingleAsync(x => x.Id == bookId.Value);

            storageBook.BookStatus = newBookStatus;
        }
        catch
        {
            throw new InvalidOperationException(
                $"No such book with id {bookId.Value}.");
        }
    }

    public async Task AddNewKeyWordsForBook(
        Id<DomainBook> bookId,
        IReadOnlySet<Id<DomainKeyWord>> keyWordsIds)
    {
        ArgumentNullException.ThrowIfNull(bookId);
        ArgumentNullException.ThrowIfNull(keyWordsIds);

        try
        {
            var storageBook = await Context.Books
                .SingleAsync(x => x.Id == bookId.Value);

            storageBook.KeywordsLinks =
                keyWordsIds
                    .Select(x => new KeyWordLink()
                    {
                        BookId = storageBook.Id,
                        KeyWordId = x.Value
                    })
                    .ToHashSet();
        }
        catch
        {
            throw new InvalidOperationException(
                $"No such book with id {bookId.Value}.");
        }
    }
}