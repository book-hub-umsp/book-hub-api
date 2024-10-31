using BookHub.Models;
using BookHub.Models.Books;
using BookHub.Models.Users;

namespace BookHub.Abstractions.Repositories;

/// <summary>
/// Описывает репозиторий для книги.
/// </summary>
public interface IBooksRepository
{
    public Task AddBookAsync(
        Book book,
        CancellationToken token);

    public Task<Book> GetBookAsync(
        Id<Book> id,
        CancellationToken token);

    public Task<bool> IsBookRelatedForCurrentAuthorAsync(
        Id<Book> bookId,
        Id<User> authorId,
        CancellationToken token);

    public Task UpdateBookDescriptionAsync(
        Id<Book> bookId,
        BookDescription newBookDescription,
        CancellationToken token);

    public Task UpdateBookStatusAsync(
        Id<Book> bookId,
        BookStatus newBookStatus,
        CancellationToken token);

    public Task UpdateBookGenreAsync(
        Id<Book> bookId,
        BookGenre newBookGenre,
        CancellationToken token);

    public Task UpdateBookAnnotationAsync(
        Id<Book> bookId,
        BookAnnotation newBookAnnotation,
        CancellationToken token);

    public Task UpdateKeyWordsForBookAsync(
        Id<Book> bookId,
        IReadOnlySet<KeyWord> keyWords,
        CancellationToken token);
}