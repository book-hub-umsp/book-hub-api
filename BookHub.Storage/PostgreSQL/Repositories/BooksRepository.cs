using System.ComponentModel;

using BookHub.Models;
using BookHub.Models.Books;
using BookHub.Models.CRUDS.Requests;
using BookHub.Models.Users;
using BookHub.Storage.PostgreSQL.Abstractions;
using BookHub.Storage.PostgreSQL.Abstractions.Repositories;

using Microsoft.EntityFrameworkCore;

using DomainBook = BookHub.Models.Books.Book;
using DomainBookGenre = BookHub.Models.Books.BookGenre;
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
    public BooksRepository(
        IRepositoryContext context,
        IKeyWordsConverter keyWordsConverter)
        : base(context)
    {
        _keyWordsConverter = keyWordsConverter
            ?? throw new ArgumentNullException(nameof(keyWordsConverter));
    }

    public async Task AddBookAsync(
        AddBookParams addBookParams,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(addBookParams);

        var relatedBookGenre =
            await Context.Genres.AsNoTracking()
                .SingleOrDefaultAsync(x => x.Value == addBookParams.Genre.Value)
                ?? throw new InvalidOperationException(
                    $"Book genre {addBookParams.Genre} is not exists.");

        var now = DateTimeOffset.UtcNow;

        var storageBook = addBookParams is AddAuthorBookParams authorBookParams
            ? new StorageBook
            {
                AuthorId = authorBookParams.AuthorId.Value,
                Title = addBookParams.Title.Value,
                BookGenreId = relatedBookGenre.Id,
                BookAnnotation = addBookParams.Annotation.Content,
                BookStatus = BookStatus.Published,
                CreationDate = now,
                LastEditDate = now
            }
            : new StorageBook
            {
                Title = addBookParams.Title.Value,
                BookGenreId = relatedBookGenre.Id,
                BookAnnotation = addBookParams.Annotation.Content,
                BookStatus = BookStatus.Published,
                CreationDate = now,
                LastEditDate = now
            };

        if (addBookParams.Keywords is not null)
        {
            storageBook.KeyWordsContent =
                _keyWordsConverter.ConvertToStorage(addBookParams.Keywords);
        }

        Context.Books.Add(storageBook);
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