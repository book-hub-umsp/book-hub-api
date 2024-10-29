using BookHub.Models;
using BookHub.Models.Books;
using BookHub.Models.Users;
using BookHub.Storage.PostgreSQL.Models;

using DomainBook = BookHub.Models.Books.Book;
using DomainKeyWord = BookHub.Models.Books.KeyWord;
using DomainBookGenre = BookHub.Models.Books.BookGenre;

namespace BookHub.Storage.PostgreSQL.Abstractions.Repositories;

/// <summary>
/// Описывает репозиторий для книги.
/// </summary>
public interface IBooksRepository
{
    public Task AddBookAsync(
        DomainBook book,
        CancellationToken token);

    public Task<DomainBook> GetBookAsync(
        Id<DomainBook> id,
        CancellationToken token);

    public Task<bool> IsBookRelatedForCurrentAuthorAsync(
        Id<DomainBook> bookId, 
        Id<User> authorId,
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
        DomainBookGenre newBookGenre, 
        CancellationToken token);

    public Task UpdateBookAnnotationAsync(
        Id<DomainBook> bookId, 
        BookAnnotation newBookAnnotation,
        CancellationToken token);

    public Task UpdateKeyWordsForBookAsync(
        Id<DomainBook> bookId, 
        IReadOnlySet<DomainKeyWord> keyWords,
        CancellationToken token);
}