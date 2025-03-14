using BookHub.API.Models.API;
using BookHub.API.Models.Books;
using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.DomainEvents.Books;
using BookHub.API.Models.Identifiers;

namespace BookHub.API.Abstractions.Logic.Services.Books.Repository;

/// <summary>
/// Описывает сервис обработки верхнеуровневого описания книги.
/// </summary>
public interface IBookService
{
    public Task AddBookAsync(
        CreatingBook creatingBook,
        CancellationToken token);

    public Task UpdateBookAsync(
        BookUpdatedBase bookUpdated,
        CancellationToken token);

    public Task<Book> GetBookByIdAsync(
        Id<Book> id,
        CancellationToken token);

    public Task<NewsItems<BookPreview>> GetBooksPreviewsAsync(
        DataManipulation dataManipulation,
        CancellationToken token);
}