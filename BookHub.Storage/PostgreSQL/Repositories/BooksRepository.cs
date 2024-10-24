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
    RepositoryBase, 
    IBooksRepository
{
    public BooksRepository(IRepositoryContext context)
        : base(context) {}

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

        var storageBook =
            await Context.Books
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id.Value, token);

        if (storageBook is null)
        {
            throw new InvalidOperationException(
                $"No such book with id {id.Value}.");
        }

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

    public async Task<bool> IsBookRelatedForCurrentAuthorAsync(
        Id<DomainBook> bookId,
        Id<Author> authorId,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookId);
        ArgumentNullException.ThrowIfNull(authorId);

        var storageBook = await Context.Books
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == bookId.Value, token);

        if (storageBook is null)
        {
            throw new InvalidOperationException(
               $"No such book with id {bookId.Value}.");
        }

        return storageBook.AuthorId == authorId.Value;
    }

    public async Task UpdateBookAnnotationAsync(
        Id<DomainBook> bookId,
        BookAnnotation newBookAnnotation,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookId);
        ArgumentNullException.ThrowIfNull(newBookAnnotation);

        var storageBook = await Context.Books
            .SingleOrDefaultAsync(x => x.Id == bookId.Value, token);

        if (storageBook is null)
        {
            throw new InvalidOperationException(
               $"No such book with id {bookId.Value}.");
        }

        storageBook.BookAnnotation = newBookAnnotation.Content;
    }

    public async Task UpdateBookDescriptionAsync(
        Id<DomainBook> bookId,
        BookDescription newBookDescription,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookId);
        ArgumentNullException.ThrowIfNull(newBookDescription);

        var storageBook = await Context.Books
            .SingleOrDefaultAsync(x => x.Id == bookId.Value, token);

        if (storageBook is null)
        {
            throw new InvalidOperationException(
               $"No such book with id {bookId.Value}.");
        }

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

        var storageBook = await Context.Books
            .SingleOrDefaultAsync(x => x.Id == bookId.Value, token);

        if (storageBook is null)
        {
            throw new InvalidOperationException(
               $"No such book with id {bookId.Value}.");
        }

        storageBook.BookStatus = newBookStatus;
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

        var storageBook = await Context.Books
            .SingleOrDefaultAsync(x => x.Id == bookId.Value, token);

        if (storageBook is null)
        {
            throw new InvalidOperationException(
               $"No such book with id {bookId.Value}.");
        }

        storageBook.BookGenre = newBookGenre;
    }

    public async Task AddNewKeyWordsForBookAsync(
        Id<DomainBook> bookId,
        IReadOnlySet<Id<DomainKeyWord>> keyWordsIds,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookId);
        ArgumentNullException.ThrowIfNull(keyWordsIds);

        var storageBook = await Context.Books
            .SingleOrDefaultAsync(x => x.Id == bookId.Value, token);

        if (storageBook is null)
        {
            throw new InvalidOperationException(
               $"No such book with id {bookId.Value}.");
        }

        storageBook.KeywordsLinks =
            keyWordsIds
                .Select(x => new KeyWordLink()
                {
                    BookId = storageBook.Id,
                    KeyWordId = x.Value
                })
                .ToHashSet();
    }
}