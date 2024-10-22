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
    public Task<Id<DomainBook>> AddBookAsync(DomainBook book);

    public Task<DomainBook> GetBook(Id<DomainBook> id);

    public Task<bool> IsBookRelatedForCurrentAuthor(
        Id<DomainBook> bookId, 
        Id<Author> authorId);

    public Task UpdateBookDescription(
        Id<DomainBook> bookId, 
        BookDescription newBookDescription);

    public Task UpdateBookStatus(
        Id<DomainBook> bookId, 
        BookStatus newBookStatus);

    public Task UpdateBookGenre(
        Id<DomainBook> bookId,
        BookGenre newBookGenre);

    public Task UpdateBookAnnotation(
        Id<DomainBook> bookId, 
        BookAnnotation newBookAnnotation);

    public Task AddNewKeyWordsForBook(
        Id<DomainBook> bookId, 
        IReadOnlySet<Id<DomainKeyWord>> keyWordsIds);
}