using BookHub.Models;
using BookHub.Models.Books;
using BookHub.Storage.PostgreSQL.Abstractions;
using BookHub.Storage.PostgreSQL.Abstractions.Repositories;
using BookHub.Storage.PostgreSQL.Models;

using DomainBook = BookHub.Models.Books.Book;
using DomainKeyWord = BookHub.Models.Books.KeyWord;

namespace BookHub.Storage.PostgreSQL.Repositories;

/// <summary>
/// Репозиторий книг.
/// </summary>
public sealed class BooksRepository :
    RepositoriesBase, IBooksRepository
{
    public BooksRepository(IRepositoryContext context) 
        : base(context)
    {
    }

    public Task AddBookAsync(DomainBook book)
    {
        throw new NotImplementedException();
    }

    public Task<DomainBook> GetInfoAboutBook(Id<DomainBook> bookId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsBookRelatedForCurrentAuthor(Id<DomainBook> bookId, Id<Author> authorId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateBookAnnotation(Id<DomainBook> bookId, BookAnnotation newBookAnnotation)
    {
        throw new NotImplementedException();
    }

    public Task UpdateBookDescription(Id<DomainBook> bookId, BookDescription newBookDescription)
    {
        throw new NotImplementedException();
    }

    public Task UpdateBookStatus(Id<DomainBook> bookId, BookStatus newBookStatus)
    {
        throw new NotImplementedException();
    }

    public Task AddNewKeyWordsForBook(IReadOnlySet<Id<DomainKeyWord>> keyWordsIds)
    {
        throw new NotImplementedException();
    }
}