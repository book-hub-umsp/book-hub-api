using BookHub.API.Abstractions.Storage.Repositories;
using BookHub.API.Models;
using BookHub.API.Models.Account;
using BookHub.API.Models.API;
using BookHub.API.Models.Books;
using BookHub.API.Models.Books.Content;
using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.DomainEvents.Books;
using BookHub.API.Models.Identifiers;
using BookHub.API.Storage.PostgreSQL.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace BookHub.API.Storage.PostgreSQL.Repositories;

/// <summary>
/// Репозиторий книг.
/// </summary>
public sealed class BooksRepository :
    RepositoryBase,
    IBooksRepository
{
    public BooksRepository(
        IRepositoryContext context)
        : base(context)
    {
    }

    public async Task AddBookAsync(
        Id<User> userId,
        CreatingBook creatingBook,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(creatingBook);

        token.ThrowIfCancellationRequested();

        _ = await Context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(
                u => u.Id == userId.Value,
                token)
            ?? throw new InvalidOperationException(
                $"User with id {userId.Value} doesn't exist.");

        _ = await Context.Genres
            .AsNoTracking()
            .SingleOrDefaultAsync(
                x => creatingBook.GenreId.Value == x.Id,
                token)
            ?? throw new InvalidOperationException(
                $"Book genre {creatingBook.GenreId} is not exists.");

        var now = DateTimeOffset.UtcNow;

        Context.Books.Add(new()
        {
            AuthorId = userId.Value,
            Title = creatingBook.Title.Value,
            BookGenreId = creatingBook.GenreId.Value,
            BookAnnotation = creatingBook.Annotation.Content,
            BookStatus = BookStatus.Published,
            CreationDate = now,
            LastEditDate = now
        });
    }

    public async Task<Book> GetBookByIdAsync(
        Id<Book> id,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(id);

        token.ThrowIfCancellationRequested();

        var storageBook = await Context.Books
            .AsNoTracking()
            .Include(x => x.KeywordLinks)
            .ThenInclude(link => link.Keyword)
            .SingleOrDefaultAsync(x => x.Id == id.Value, token)
                ?? throw new InvalidOperationException(
                    $"No such book with id {id.Value}.");

        var domainKeywords = storageBook.KeywordLinks
            .Select(x => new KeyWord(new(x.Keyword.Value)))
            .ToHashSet();

        return new Book(
            new(storageBook.Id),
            new(storageBook.AuthorId),
            new(new(storageBook.BookGenre.Value),
                new(storageBook.Title),
                new(storageBook.BookAnnotation),
                domainKeywords),
            storageBook.BookStatus);
    }

    public async Task UpdateBookAsync(
        Id<User> userId,
        BookUpdatedBase bookUpdated,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookUpdated);

        var storageBook = await Context.Books
            .SingleOrDefaultAsync(x =>
                x.Id == bookUpdated.EntityId.Value
                && x.AuthorId == userId.Value, token)
            ?? throw new InvalidOperationException(
                $"No such book with id {bookUpdated.EntityId.Value} for user {userId.Value}.");

        switch (bookUpdated)
        {
            case BookUpdated<Id<BookGenre>> update:
                storageBook.BookGenreId = update.Attribute.Value;
                break;

            case BookUpdated<Name<Book>> update:
                storageBook.Title = update.Attribute.Value;
                break;

            case BookUpdated<BookAnnotation> update:
                storageBook.BookAnnotation = update.Attribute.Content;
                break;

            case BookUpdated<BookStatus> update:
                storageBook.BookStatus = update.Attribute;
                break;

            case BookUpdated<IReadOnlyCollection<Id<KeyWord>>> update:
                storageBook.KeywordLinks = update.Attribute
                    .Select(x => new Models.KeywordLink
                    {
                        BookId = storageBook.Id,
                        KeywordId = x.Value
                    })
                    .ToList();
                break;
        };
    }

    public async Task<IReadOnlyCollection<BookPreview>> GetBooksPreviewAsync(
        DataManipulation dataManipulation,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(dataManipulation);

        token.ThrowIfCancellationRequested();

        var storageBook = await Context.Books
            .AsNoTracking()
            .Include(x => x.BookGenre)
            .Include(x => x.Partitions.Select(x => x.SequenceNumber))
            .WithFiltering(dataManipulation.Filters)
            .WithPaging(dataManipulation.Pagination)
            .ToListAsync(token);

        return storageBook
            .Select(x => new BookPreview(
                new(x.Id),
                new(x.Title),
                new(x.BookGenre.Value),
                new(x.AuthorId),
                x.Partitions
                    .Select(x => new PartitionSequenceNumber(x.SequenceNumber))
                    .ToHashSet()))
            .ToList();
    }

    public async Task<bool> IsUserAuthorForBook(
        Id<User> userId,
        Id<Book> bookId,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(bookId);

        token.ThrowIfCancellationRequested();

        var book = await Context.Books
            .AsNoTracking()
            .Select(x => new { x.Id, x.AuthorId })
            .SingleOrDefaultAsync(x => x.Id == bookId.Value, token)
            ?? throw new InvalidOperationException(
                $"Book {bookId.Value} is not found.");

        return book.AuthorId == userId.Value;
    }

    public async Task<long> GetBooksTotalCountAsync(CancellationToken token) =>
        await Context.Books.LongCountAsync(token);
}