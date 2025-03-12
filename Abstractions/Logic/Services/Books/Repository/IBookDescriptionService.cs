using BookHub.API.Models;
using BookHub.API.Models.Account;
using BookHub.API.Models.API;
using BookHub.API.Models.API.Pagination;
using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.CRUDS.Requests;

namespace BookHub.API.Abstractions.Logic.Services.Books.Repository;

/// <summary>
/// Описывает сервис обработки верхнеуровневого описания книги.
/// </summary>
public interface IBookDescriptionService
{
    public Task AddBookAsync(
        AddBookParams addBookParams,
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