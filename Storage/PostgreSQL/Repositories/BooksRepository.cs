using System.Linq.Expressions;

using BookHub.API.Abstractions.Storage.Repositories;
using BookHub.API.Models;
using BookHub.API.Models.API.Pagination;
using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.CRUDS.Requests;
using BookHub.API.Storage.PostgreSQL;
using BookHub.API.Storage.PostgreSQL.Abstractions;
using BookHub.API.Storage.PostgreSQL.Models;
using BookHub.API.Storage.PostgreSQL.Models.Previews;
using BookHub.API.Storage.PostgreSQL.Repositories;

using Microsoft.EntityFrameworkCore;

using DomainBook = BookHub.API.Models.Books.Repository.Book;
using DomainUser = BookHub.API.Models.Account.User;
using StorageBook = BookHub.API.Storage.PostgreSQL.Models.Book;

namespace BookHub.Storage.PostgreSQL.Repositories;

/// <summary>
/// Репозиторий книг.
/// </summary>
public sealed partial class BooksRepository :
    RepositoryBase,
    IBooksRepository
{
    public BooksRepository(
        IRepositoryContext context)
        : base(context)
    {
    }

    public async Task AddBookAsync(
        Id<DomainUser> userId,
        AddBookParams addBookParams,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(addBookParams);

        _ = await Context.Users
            .SingleOrDefaultAsync(
                u => u.Id == userId.Value,
                token)
            ?? throw new InvalidOperationException(
                $"User with id {userId.Value} doesn't exist.");

        var relatedBookGenre =
            await Context.Genres.AsNoTracking()
                .SingleOrDefaultAsync(
                    x => addBookParams.Genre.Value == x.Value,
                    token)
                ?? throw new InvalidOperationException(
                    $"Book genre {addBookParams.Genre} is not exists.");

        var now = DateTimeOffset.UtcNow;

        var storageBook = new StorageBook
        {
            AuthorId = userId.Value,
            Title = addBookParams.Title.Value,
            BookGenreId = relatedBookGenre.Id,
            BookAnnotation = addBookParams.Annotation.Content,
            BookStatus = BookStatus.Published,
            CreationDate = now,
            LastEditDate = now
        };

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

        var domainKeywords =
            storageBook.KeywordLinks?
                .Select(x => new KeyWord(new(x.Keyword.Value)))
                .ToHashSet();

        var domainDescription = domainKeywords is null
            ? new BookDescription(
                new(storageBook.BookGenre.Value),
                new(storageBook.Title),
                new(storageBook.BookAnnotation))
            : new BookDescription(
                new(storageBook.BookGenre.Value),
                new(storageBook.Title),
                new(storageBook.BookAnnotation),
                domainKeywords);

        return new DomainBook(
                new(storageBook.Id),
                new(storageBook.AuthorId),
                domainDescription,
                storageBook.BookStatus);
    }

    public async Task UpdateBookContentAsync(
        Id<DomainUser> userId,
        UpdateBookParamsBase updateBookParams,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(updateBookParams);

        _ = await Context.Users
            .SingleOrDefaultAsync(
                u => u.Id == userId.Value,
                token)
            ?? throw new InvalidOperationException(
                $"User with id {userId.Value} doesn't exist.");

        var storageBook = await Context.Books
            .SingleOrDefaultAsync(x => x.Id == updateBookParams.BookId.Value, token)
                ?? throw new InvalidOperationException(
                    $"No such book with id {updateBookParams.BookId.Value}.");

        if (storageBook.AuthorId != userId.Value)
        {
            throw new InvalidOperationException(
                $"Only author can update book {storageBook.Id} description.");
        }

        switch (updateBookParams)
        {
            case UpdateBookGenreParams genreParams:

                var newGenre = genreParams.NewGenre;

                var relatedBookGenre =
                    await Context.Genres.AsNoTracking()
                        .SingleOrDefaultAsync(
                            x => newGenre.Value == x.Value,
                            token)
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

            case ExtendKeyWordsParams keyWordsParams:

                await SynchronizeKeywordsAsync(
                    storageBook,
                    keyWordsParams.UpdatedKeyWords,
                    token);

                break;

            default:
                throw new InvalidOperationException(
                $"Not supported params type {updateBookParams.GetType().Name}.");
        }
    }

    public async Task<IReadOnlyCollection<BookPreview>> GetAuthorBooksAsync(
        Id<DomainUser> authorId,
        PaggingBase pagination,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(pagination);

        _ = await Context.Users
            .SingleOrDefaultAsync(
                u => u.Id == authorId.Value,
                token)
            ?? throw new InvalidOperationException(
                $"User with id {authorId.Value} doesn't exist.");

        var booksShortModels =
            await Context.Books.AsNoTracking()
                .Where(x => x.AuthorId == authorId.Value)
                .WithPaging(pagination)
                .GroupJoinOfStorageBookPreviews(Context.Chapters)
                .ToListAsync(token);

        return booksShortModels
            .Select(StorageBookPreview.ToDomain)
            .ToList();
    }

    public async Task<IReadOnlyCollection<BookPreview>> GetBooksAsync(
        PaggingBase pagination,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(pagination);

        var booksShortModels =
            await Context.Books.AsNoTracking()
                .WithPaging(pagination)
                .GroupJoinOfStorageBookPreviews(Context.Chapters)
                .ToListAsync(token);

        return booksShortModels
            .Select(StorageBookPreview.ToDomain)
            .ToList();
    }

    public async Task<IReadOnlyCollection<BookPreview>> GetBooksPreviewsAsync(
        IReadOnlySet<Id<DomainBook>> bookIds,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(bookIds);

        Expression<Func<StorageBook, bool>> booksIdsPredicate =
            book => bookIds.Select(x => x.Value).Any(x => x == book.Id);

        var booksShortModels =
            await Context.Books
                .AsNoTracking()
                .Where(booksIdsPredicate)
                .GroupJoinOfStorageBookPreviews(Context.Chapters)
                .ToListAsync(token);

        return booksShortModels
            .Select(StorageBookPreview.ToDomain)
            .ToList();
    }

    public async Task<IReadOnlyCollection<BookPreview>> GetBooksByKeywordAsync(
        KeyWord keyword,
        PaggingBase pagination,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(keyword);
        ArgumentNullException.ThrowIfNull(pagination);

        var stringMatchExpression = BuildStringContainsExpression(keyword.Content.Value);

        var keywordMatchExpression = CombineExpressions<KeywordLink>(
            keywordLink => keywordLink.Keyword.Value, stringMatchExpression);

        var booksShortModels =
            await Context.Books
                .AsNoTracking()
                .Include(book => book.KeywordLinks)
                .Where(book => book.KeywordLinks.AsQueryable().Any(keywordMatchExpression))
                .WithPaging(pagination)
                .GroupJoinOfStorageBookPreviews(Context.Chapters)
                .ToListAsync(token);

        return booksShortModels
            .Select(StorageBookPreview.ToDomain)
            .ToList();
    }

    public async Task<bool> IsUserAuthorForBook(
        Id<DomainUser> userId,
        Id<DomainBook> bookId,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(bookId);

        var book = await Context.Books
            .AsNoTracking()
            .Select(x => new { x.Id, x.AuthorId })
            .SingleOrDefaultAsync(x => x.Id == bookId.Value, token)
            ?? throw new InvalidOperationException(
                $"Book {bookId.Value} is not found.");

        return book.AuthorId == userId.Value;
    }

    public async Task SynchronizeKeyWordsForBook(
        Id<DomainUser> userId,
        IReadOnlySet<KeyWord> keyWords,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(keyWords);

        _ = await Context.Users
            .SingleOrDefaultAsync(
                u => u.Id == userId.Value,
                token)
            ?? throw new InvalidOperationException(
                $"User with id {userId.Value} doesn't exist.");

        if (!keyWords.Any())
        {
            return;
        }

        var lastBookForUser =
            await Context.Books
                .OrderBy(x => x.Id)
                .LastOrDefaultAsync(
                    x => x.AuthorId == userId.Value,
                    token)
            ?? throw new InvalidOperationException(
                $"No one book for user with id {userId.Value} was found.");

        await SynchronizeKeywordsAsync(lastBookForUser, keyWords, token);
    }

    public async Task<long> GetBooksTotalCountAsync(CancellationToken token) =>
        await Context.Books.LongCountAsync(token);

    private async Task SynchronizeKeywordsAsync(
        StorageBook storageBook,
        IReadOnlySet<KeyWord> keywords,
        CancellationToken token)
    {
        foreach (var keyword in keywords)
        {
            var existedKeyword = await Context.Keywords
                .AsNoTracking()
                .SingleOrDefaultAsync(
                    x => x.Value == keyword.Content.Value,
                    token);

            if (existedKeyword is not null)
            {
                if (storageBook.KeywordLinks.Any(
                    x => x.KeywordId == existedKeyword.Id))
                {
                    return;
                }

                Context.KeywordLinks.Add(
                    new KeywordLink
                    {
                        Keyword = existedKeyword,
                        BookId = storageBook.Id
                    });

                continue;
            }

            var newKeyword = new Keyword
            {
                Value = keyword.Content.Value
            };

            Context.Keywords.Add(newKeyword);

            Context.KeywordLinks.Add(
                new KeywordLink
                {
                    Keyword = newKeyword,
                    BookId = storageBook.Id
                });
        }
    }
}