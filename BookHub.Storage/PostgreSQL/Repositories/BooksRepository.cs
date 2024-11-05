using BookHub.Abstractions.Storage;
using BookHub.Abstractions.Storage.Repositories;
using BookHub.Models;
using BookHub.Models.Books;
using BookHub.Models.CRUDS.Requests;
using BookHub.Models.Users;
using BookHub.Storage.PostgreSQL.Abstractions;

using Microsoft.EntityFrameworkCore;

using DomainBook = BookHub.Models.Books.Book;
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
        AddAuthorBookParams addBookParams,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(addBookParams);

        var relatedBookGenre =
            await Context.Genres.AsNoTracking()
                .SingleOrDefaultAsync(x => addBookParams.Genre.CompareTo(x.Value))
                ?? throw new InvalidOperationException(
                    $"Book genre {addBookParams.Genre} is not exists.");

        var now = DateTimeOffset.UtcNow;

        var storageBook = new StorageBook
        {
            AuthorId = addBookParams.AuthorId.Value,
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

    public async Task UpdateBookContentAsync(
        UpdateBookParamsBase updateBookParams,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(updateBookParams);

        var storageBook = await Context.Books
            .SingleOrDefaultAsync(x => x.Id == updateBookParams.BookId.Value, token)
                ?? throw new InvalidOperationException(
                    $"No such book with id {updateBookParams.BookId.Value}.");

        switch (updateBookParams)
        {
            case UpdateBookGenreParams genreParams:

                var newGenre = genreParams.NewGenre;

                var relatedBookGenre =
                    await Context.Genres.AsNoTracking()
                        .SingleOrDefaultAsync(x => newGenre.CompareTo(x.Value))
                            ?? throw new InvalidOperationException(
                                $"Book genre {newGenre} is not exists.");

                storageBook.BookGenreId = relatedBookGenre.Id;

                break;

            case UpdateBookTitleParams titleParams:

                storageBook.Title = titleParams.NewTitle.Value;

                break;

            case UpdateBookAnnotationParams annotationParams:

                storageBook.BookAnnotation = annotationParams.NewBookAnnotation.Content;

                break;

            case UpdateBookStatusParams statusParams:

                storageBook.BookStatus = statusParams.NewBookStatus;

                break;

            case UpdateKeyWordsParams keyWordsParams:

                storageBook.KeyWordsContent =
                    _keyWordsConverter.ConvertToStorage(keyWordsParams.UpdatedKeyWords);

                break;

            default:
                throw new InvalidOperationException(
                $"Not supported params type {updateBookParams.GetType().Name}.");
        }
    }

    private readonly IKeyWordsConverter _keyWordsConverter;
}