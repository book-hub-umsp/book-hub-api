using BookHub.Abstractions.Storage.Repositories;
using BookHub.Models;
using BookHub.Models.Account;
using BookHub.Models.API.Pagination;
using BookHub.Models.Books.Content;
using BookHub.Models.Books.Repository;
using BookHub.Models.Favorite;
using BookHub.Storage.PostgreSQL.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace BookHub.Storage.PostgreSQL.Repositories;

/// <summary>
/// Репозиторий избранного.
/// </summary>
public sealed class FavoriteLinkRepository : 
    RepositoryBase, 
    IFavoriteLinkRepository
{
    public FavoriteLinkRepository(
        IRepositoryContext context) 
        : base(context) { }

    public async Task AddFavoriteLinkAsync(
        UserFavoriteBookLink favoriteLink, 
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(favoriteLink);

        var user = await Context.Users
            .SingleOrDefaultAsync(
                u => u.Id == favoriteLink.UserId.Value, 
                token)
            ?? throw new InvalidOperationException(
                $"User with id {favoriteLink.UserId.Value} doesn't exist.");

        var book = await Context.Books
            .SingleOrDefaultAsync(
                b => b.Id == favoriteLink.BookId.Value, 
                token)
            ?? throw new InvalidOperationException(
                $"Book with id {favoriteLink.BookId.Value} doesn't exist.");

        var userFavorite = await Context.FavoriteLinks
            .SingleOrDefaultAsync(
                u => u.UserId == user.Id && u.BookId == book.Id,
                token);

        if (userFavorite != null)
        {
            throw new InvalidOperationException(
                "User favorite with bookId" +
                $" {favoriteLink.BookId.Value} already exists.");
        }

        Context.FavoriteLinks.Add(new()
        {
            UserId = favoriteLink.UserId.Value,
            User = user,
            BookId = favoriteLink.BookId.Value,
            Book = book,
        });
    }

    public async Task<IReadOnlyCollection<BookPreview>> GetUsersFavoriteAsync(
        Id<User> userId, 
        PagePagging pagePagination, 
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(pagePagination);

        var user = await Context.Users
            .SingleOrDefaultAsync(
                u => u.Id == userId.Value,
                token)
            ?? throw new InvalidOperationException(
                $"User with id {userId.Value} doesn't exist.");

        var storageFavLinks = await Context.FavoriteLinks
            .Select(x => new { x.BookId, x.UserId })
            .Where(f => f.UserId == user.Id)
            .OrderBy(x => x.BookId)
            .Skip(pagePagination.PageSize * (pagePagination.PageNumber - 1))
            .Take(pagePagination.PageSize)
            .ToListAsync(token);

        var favouritesBooksIds = storageFavLinks.Select(x => x.BookId).ToHashSet();

        var favoriteBookShortModels = 
            await Context.Books.AsNoTracking()
                .Where(book => favouritesBooksIds.Contains(book.Id))
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

        return favoriteBookShortModels
            .Select(x => new BookPreview(
                new(x.BookId),
                new(x.Title),
                new(x.BookGenre.Value),
                new(x.AuthorId),
                x.ChapterPreview
                    .ToDictionary(
                        k => new Id<Chapter>(k.ChapterId),
                        v => new ChapterSequenceNumber(v.SequenceNumber))
                ))
            .ToList();
    }

    public async Task RemoveFavoriteLinkAsync(
        UserFavoriteBookLink favoriteLink, 
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(favoriteLink);

        var userFavorite = await Context.FavoriteLinks
            .FindAsync(
                [favoriteLink.UserId.Value, favoriteLink.BookId.Value],
                token)
            ?? throw new InvalidOperationException(
                "User favorite with bookId" +
                $" {favoriteLink.BookId.Value} doesn't exists.");

        Context.FavoriteLinks.Remove(userFavorite);
    }

    public async Task<long> GetTotalCountFavoriteLinkAsync(
        CancellationToken token) =>
        await Context.FavoriteLinks.LongCountAsync(token);
}