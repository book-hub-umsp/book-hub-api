using BookHub.Models;
using BookHub.Models.Books;
using BookHub.Storage.PostgreSQL.Models;

using DomainBook = BookHub.Models.Books.Book;
using DomainKeyWord = BookHub.Models.Books.KeyWord;

namespace BookHub.Storage.PostgreSQL.Abstractions.Repositories;

/// <summary>
/// Описывает репозиторий для книги.
/// </summary>
public interface IBooksRepository
{
    public Task<Id<DomainBook>> AddBookAsync(
        DomainBook book,
        CancellationToken token);

    public Task<DomainBook> GetBookAsync(
        Id<DomainBook> id,
        CancellationToken token);

    public Task<bool> IsBookRelatedForCurrentAuthorAsync(
        Id<DomainBook> bookId, 
        Id<Author> authorId,
        CancellationToken token);

    public Task UpdateBookDescriptionAsync(
        Id<DomainBook> bookId, 
        BookDescription newBookDescription,
        CancellationToken token);

    public Task UpdateBookStatusAsync(
        Id<DomainBook> bookId, 
        BookStatus newBookStatus,
        CancellationToken token);

    public Task UpdateBookGenreAsync(
        Id<DomainBook> bookId,
        BookGenre newBookGenre, 
        CancellationToken token);

    public Task UpdateBookAnnotationAsync(
        Id<DomainBook> bookId, 
        BookAnnotation newBookAnnotation,
        CancellationToken token);

    public Task AddNewKeyWordsForBookAsync(
        Id<DomainBook> bookId, 
        IReadOnlySet<Id<DomainKeyWord>> keyWordsIds,
        CancellationToken token);
}