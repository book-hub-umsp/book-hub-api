using System.Linq.Expressions;

using BookHub.Abstractions.Storage.Repositories;
using BookHub.Models;
using BookHub.Models.API.Pagination;
using BookHub.Models.Books.Content;
using BookHub.Models.Books.Repository;
using BookHub.Models.CRUDS.Requests;
using BookHub.Storage.PostgreSQL.Abstractions;
using BookHub.Storage.PostgreSQL.Models;

using Microsoft.EntityFrameworkCore;

using DomainBook = BookHub.Models.Books.Repository.Book;
using DomainUser = BookHub.Models.Account.User;
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
        IRepositoryContext context)
        : base(context)
    {
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
            await SynchronizeKeywordsAsync(
                addBookParams.Keywords,
                storageBook,
                token);
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

    public async Task<bool> IsBookRelatedForCurrentAuthorAsync(
        Id<DomainBook> bookId,
        Id<DomainUser> authorId,
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

        if (storageBook.AuthorId != updateBookParams.AuthorId.Value)
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

                await SynchronizeKeywordsAsync(
                    keyWordsParams.UpdatedKeyWords,
                    storageBook,
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

        var booksShortModels =
            await Context.Books.AsNoTracking()
                .Where(x => x.AuthorId == authorId.Value)
                .WithPaging(pagination)
                .GroupJoin(
                    Context.Chapters.Select(x => new { ChapterId = x.Id, x.BookId, x.SequenceNumber }),
                    x => x.Id,
                    x => x.BookId,
                    (book, chapter) => new 
                    { 
                        BookId = book.Id,
                        book.Title,
                        book.BookGenre,
                        book.AuthorId,
                        ChapterPreview = chapter 
                    })
                .ToListAsync(token);

        return booksShortModels
            .Select(x => new BookPreview(
                new(x.BookId),
                new(x.Title),
                new(x.BookGenre.Value),
                new(x.AuthorId),
                x.ChapterPreview
                    .ToDictionary(
                        k => new Id<BookHub.Models.Books.Content.Chapter>(k.ChapterId),
                        v => new ChapterSequenceNumber(v.SequenceNumber))
                ))
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
                .GroupJoin(
                    Context.Chapters.Select(x => new { ChapterId = x.Id, x.BookId, x.SequenceNumber }),
                    x => x.Id,
                    x => x.BookId,
                    (book, chapter) => new
                    {
                        BookId = book.Id,
                        book.Title,
                        book.BookGenre,
                        book.AuthorId,
                        ChapterPreview = chapter
                    })
                .ToListAsync(token);

        return booksShortModels
            .Select(x => new BookPreview(
                new(x.BookId),
                new(x.Title),
                new(x.BookGenre.Value),
                new(x.AuthorId),
                x.ChapterPreview
                    .ToDictionary(
                        k => new Id<BookHub.Models.Books.Content.Chapter>(k.ChapterId),
                        v => new ChapterSequenceNumber(v.SequenceNumber))
                ))
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
                .GroupJoin(
                    Context.Chapters.Select(x => new { ChapterId = x.Id, x.BookId, x.SequenceNumber }),
                    x => x.Id,
                    x => x.BookId,
                    (book, chapter) => new
                    {
                        BookId = book.Id,
                        book.Title,
                        book.BookGenre,
                        book.AuthorId,
                        ChapterPreview = chapter
                    })
                .ToListAsync(token);


        return booksShortModels
            .Select(x => new BookPreview(
                new(x.BookId),
                new(x.Title),
                new(x.BookGenre.Value),
                new(x.AuthorId),
                x.ChapterPreview
                    .ToDictionary(
                        k => new Id<BookHub.Models.Books.Content.Chapter>(k.ChapterId),
                        v => new ChapterSequenceNumber(v.SequenceNumber))
                ))
            .ToList();
    }

    public async Task<long> GetBooksTotalCountAsync(CancellationToken token) =>
        await Context.Books.LongCountAsync(token);

    private async Task SynchronizeKeywordsAsync(
        IReadOnlySet<KeyWord> keywords,
        StorageBook storageBook,
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
                Context.KeywordLinks.Add(
                    new KeywordLink
                    {
                        Keyword = existedKeyword,
                        Book = storageBook
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
                    Book = storageBook
                });
        }
    }

    private static Expression<Func<T, bool>> CombineExpressions<T>(
        Expression<Func<T, string>> valueSelector,
        Expression<Func<string, bool>> condition)
    {
        var parameter = valueSelector.Parameters[0];

        var body = Expression.Invoke(condition, valueSelector.Body);

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    private static Expression<Func<string, bool>> BuildStringContainsExpression(string keyword)
    {
        var processedKeyword = keyword.Replace("_", string.Empty).ToLower();
        var parameter = Expression.Parameter(typeof(string), "value");

        var valueProcessed = Expression.Call(
            Expression.Call(parameter, nameof(string.Replace), null, Expression.Constant("_"), Expression.Constant(string.Empty)),
            nameof(string.ToLower), null);

        var body = Expression.OrElse(
            Expression.Call(valueProcessed, nameof(string.Contains), null, Expression.Constant(processedKeyword)),
            Expression.Call(Expression.Constant(processedKeyword), nameof(string.Contains), null, valueProcessed));

        return Expression.Lambda<Func<string, bool>>(body, parameter);
    }
}