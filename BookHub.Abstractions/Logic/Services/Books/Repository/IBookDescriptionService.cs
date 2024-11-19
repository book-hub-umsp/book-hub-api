using BookHub.Models;
using BookHub.Models.Account;
using BookHub.Models.API;
using BookHub.Models.API.Pagination;
using BookHub.Models.Books.Repository;
using BookHub.Models.CRUDS.Requests;

namespace BookHub.Abstractions.Logic.Services.Books.Repository;

/// <summary>
/// Описывает сервис обработки верхнеуровневого описания книги.
/// </summary>
public interface IBookDescriptionService
{
    public Task AddBookAsync(
        AddAuthorBookParams addBookParams,
        CancellationToken token);

    public Task UpdateBookAsync(
        UpdateBookParamsBase updateBookParams,
        CancellationToken token);

    public Task<Book> GetBookAsync(
        GetBookParams getBookParams,
        CancellationToken token);

    public Task<NewsItems<BookPreview>> GetAuthorBooksPreviewsAsync(
        Id<User> authorId,
        PagePagging pagination,
        CancellationToken token);

    public Task<NewsItems<BookPreview>> GetBooksPreviewsAsync(
        PagePagging pagination,
        CancellationToken token);

    public Task<NewsItems<BookPreview>> GetBooksPreviewsByKeywordAsync(
        KeyWord keyword,
        PagePagging pagination,
        CancellationToken token);
}