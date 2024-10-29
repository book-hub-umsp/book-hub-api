using BookHub.Models;
using BookHub.Models.Books;
using BookHub.Models.Users;
using BookHub.Storage.PostgreSQL.Abstractions;
using BookHub.Storage.PostgreSQL.Abstractions.Repositories;
using BookHub.Storage.PostgreSQL.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

using DomainBook = BookHub.Models.Books.Book;
using DomainKeyWord = BookHub.Models.Books.KeyWord;
using DomainBookGenre = BookHub.Models.Books.BookGenre;
using StorageBook = BookHub.Storage.PostgreSQL.Models.Book;

namespace BookHub.Storage.PostgreSQL.Repositories;

/// <summary>
/// Репозиторий книг.
/// </summary>
public sealed class BooksRepository :
    RepositoryBase, 
    IBooksRepository
{
    public BooksRepository(
        IRepositoryContext context, 
        IKeyWordsConverter keyWordsConverter)
        : base(context) 
    {
        _keyWordsConverter = keyWordsConverter 
            ?? throw new ArgumentNullException(nameof(keyWordsConverter));
    }

    public async Task AddBookAsync(
        DomainBook book,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(book);

        var relatedBookGenre = 
            await Context.Genres.AsNoTracking()
                .SingleOrDefaultAsync(x => x.Value == book.Description.Genre.Value) 
                ?? throw new InvalidOperationException(
                    $"Book genre {book.Description.Genre} is not exists.");

        var storageBook = new StorageBook
        {
            AuthorId = book.AuthorId.Value,
            Title = book.Description.Title.Value,
            BookGenreId = relatedBookGenre.Id,
            BookAnnotation = book.Description.BookAnnotation.Content,
            BookStatus = book.Status,
            CreationDate = book.CreationDate,
            LastEditDate = book.LastEditDate
        };

        Context.Books.Add(storageBook);

        storageBook.KeyWordsContent = 
            _keyWordsConverter.ConvertToStorage(book.Description.KeyWords);
    }

    public async Task<DomainBook> GetBookAsync(
        Id<DomainBook> id,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(id);

        var storageBook =
            await Context.Books
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == id.Value, token) 
                    ?? throw new InvalidOperationException(
                        $"No such book with id {id.Value}.");

        if (storageBook.BookGenre is null)
        {
            throw new InvalidOperationException(
                $"Book genre with id {storageBook.BookGenreId} is not exists.");
        }

        return new DomainBook(
                new(storageBook.Id),
                new(storageBook.AuthorId),
                new(
                    new(storageBook.BookGenre.Value),
                    new(storageBook.Title),
                    new(storageBook.BookAnnotation),
                    _keyWordsConverter
                        .ConvertToDomain(storageBook.KeyWordsContent)
                        .ToHashSet()),
                storageBook.BookStatus);
    }

    public async Task<bool> IsBookRelatedForCurrentAuthorAsync(
        Id<DomainBook> bookId,
        Id<User> authorId,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookId);
        ArgumentNullException.ThrowIfNull(authorId);

        var storageBook = await Context.Books
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == bookId.Value, token);

        return storageBook is null
            ? throw new InvalidOperationException(
               $"No such book with id {bookId.Value}.")
            : storageBook.AuthorId == authorId.Value;
    }

    public async Task UpdateBookAnnotationAsync(
        Id<DomainBook> bookId,
        BookAnnotation newBookAnnotation,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookId);
        ArgumentNullException.ThrowIfNull(newBookAnnotation);

        var storageBook = await Context.Books
            .SingleOrDefaultAsync(x => x.Id == bookId.Value, token) 
                ?? throw new InvalidOperationException(
                    $"No such book with id {bookId.Value}.");

        storageBook.BookAnnotation = newBookAnnotation.Content;
    }

    public async Task UpdateBookTitleAsync(
        Id<DomainBook> bookId,
        Name<DomainBook> newBookTitle,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookId);
        ArgumentNullException.ThrowIfNull(newBookTitle);

        var storageBook = await Context.Books
            .SingleOrDefaultAsync(x => x.Id == bookId.Value, token)
                ?? throw new InvalidOperationException(
                    $"No such book with id {bookId.Value}.");

        storageBook.Title = newBookTitle.Value;
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
                typeof(BookStatus));
        }

        var storageBook = await Context.Books
            .SingleOrDefaultAsync(x => x.Id == bookId.Value, token) 
                ?? throw new InvalidOperationException(
                    $"No such book with id {bookId.Value}.");

        storageBook.BookStatus = newBookStatus;
    }

    public async Task UpdateBookGenreAsync(
        Id<DomainBook> bookId, 
        DomainBookGenre newBookGenre,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookId);
        ArgumentNullException.ThrowIfNull(newBookGenre);

        var relatedBookGenre =
            await Context.Genres.AsNoTracking()
                .SingleOrDefaultAsync(x => x.Value == newBookGenre.Value) 
                    ?? throw new InvalidOperationException(
                        $"Book genre {newBookGenre} is not exists.");

        var storageBook = await Context.Books
            .SingleOrDefaultAsync(x => x.Id == bookId.Value, token) 
                ?? throw new InvalidOperationException(
                    $"No such book with id {bookId.Value}.");

        storageBook.BookGenreId = relatedBookGenre.Id;
    }

    public async Task UpdateKeyWordsForBookAsync(
        Id<DomainBook> bookId,
        IReadOnlySet<DomainKeyWord> keyWords,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookId);
        ArgumentNullException.ThrowIfNull(keyWords);

        var storageBook = await Context.Books
            .SingleOrDefaultAsync(x => x.Id == bookId.Value, token) 
                ?? throw new InvalidOperationException(
                    $"No such book with id {bookId.Value}.");

        storageBook.KeyWordsContent =
            _keyWordsConverter.ConvertToStorage(keyWords);
    }

    private readonly IKeyWordsConverter _keyWordsConverter;
}