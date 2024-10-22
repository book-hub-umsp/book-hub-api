using BookHub.Models;
using BookHub.Models.Books;
using BookHub.Storage.PostgreSQL.Abstractions;
using BookHub.Storage.PostgreSQL.Abstractions.Repositories;
using BookHub.Storage.PostgreSQL.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

using DomainBook = BookHub.Models.Books.Book;
using DomainKeyWord = BookHub.Models.Books.KeyWord;
using StorageBook = BookHub.Storage.PostgreSQL.Models.Book;

namespace BookHub.Storage.PostgreSQL.Repositories;

/// <summary>
/// Репозиторий книг.
/// </summary>
public sealed class BooksRepository :
    RepositoriesBase, 
    IBooksRepository
{
    public BooksRepository(IRepositoryContext context)
        : base(context)
    {
    }

    public async Task<Id<DomainBook>> AddBookAsync(
        DomainBook book,
        CancellationToken token)
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

        Context.Books.Add(storageBook);

        await Context.SaveChangesAsync(token);

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

    public async Task<DomainBook> GetBookAsync(
        Id<DomainBook> id,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(id);

        try
        {
            var storageBook =
                await Context.Books.SingleAsync(x => x.Id == id.Value, token);

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

    public async Task<bool> IsBookRelatedForCurrentAuthorAsync(
        Id<DomainBook> bookId,
        Id<Author> authorId,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookId);
        ArgumentNullException.ThrowIfNull(authorId);

        try
        {
            var storageBook = await Context.Books
                .SingleAsync(x => x.Id == bookId.Value, token);

            return storageBook.AuthorId == authorId.Value;
        }
        catch
        {
            throw new InvalidOperationException(
                $"No such book with id {bookId.Value}.");
        }
    }

    public async Task UpdateBookAnnotationAsync(
        Id<DomainBook> bookId,
        BookAnnotation newBookAnnotation,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookId);
        ArgumentNullException.ThrowIfNull(newBookAnnotation);

        try
        {
            var storageBook = await Context.Books
                .SingleAsync(x => x.Id == bookId.Value, token);

            storageBook.BookAnnotation = newBookAnnotation.Content;
        }
        catch
        {
            throw new InvalidOperationException(
                $"No such book with id {bookId.Value}.");
        }
    }

    public async Task UpdateBookDescriptionAsync(
        Id<DomainBook> bookId,
        BookDescription newBookDescription,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookId);
        ArgumentNullException.ThrowIfNull(newBookDescription);

        try
        {
            var storageBook = await Context.Books
                .SingleAsync(x => x.Id == bookId.Value, token);

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

    public async Task UpdateBookStatusAsync(
        Id<DomainBook> bookId,
        BookStatus newBookStatus,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookId);

        if (!Enum.IsDefined(newBookStatus))
        {
            throw new InvalidEnumArgumentException(
                nameof(newBookStatus),
                (int)newBookStatus,
                typeof(BookGenre));
        }

        try
        {
            var storageBook = await Context.Books
                .SingleAsync(x => x.Id == bookId.Value, token);

            storageBook.BookStatus = newBookStatus;
        }
        catch
        {
            throw new InvalidOperationException(
                $"No such book with id {bookId.Value}.");
        }
    }

    public async Task UpdateBookGenreAsync(
        Id<DomainBook> bookId, 
        BookGenre newBookGenre,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookId);

        if (!Enum.IsDefined(newBookGenre))
        {
            throw new InvalidEnumArgumentException(
                nameof(newBookGenre),
                (int)newBookGenre,
                typeof(BookGenre));
        }

        try
        {
            var storageBook = await Context.Books
                .SingleAsync(x => x.Id == bookId.Value, token);

            storageBook.BookGenre = newBookGenre;
        }
        catch
        {
            throw new InvalidOperationException(
                $"No such book with id {bookId.Value}.");
        }
    }

    public async Task AddNewKeyWordsForBookAsync(
        Id<DomainBook> bookId,
        IReadOnlySet<Id<DomainKeyWord>> keyWordsIds,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookId);
        ArgumentNullException.ThrowIfNull(keyWordsIds);

        try
        {
            var storageBook = await Context.Books
                .SingleAsync(x => x.Id == bookId.Value, token);

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