using BookHub.Abstractions.Storage.Repositories;
using BookHub.Models;
using BookHub.Models.Account;
using BookHub.Models.API.Pagination;
using BookHub.Models.Favorite;
using BookHub.Storage.PostgreSQL.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace BookHub.Storage.PostgreSQL.Repositories;

/// <summary>
/// Репозиторий избранного.
/// </summary>
public sealed class FavoriteLinkRepository : RepositoryBase, IFavoriteLinkRepository
{
    public FavoriteLinkRepository(IRepositoryContext context) : base(context) { }

    public async Task AddFavoriteLinkAsync(UserFavoriteBookLink favoriteLink, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(favoriteLink);

        var user = await Context.Users
            .SingleOrDefaultAsync(u => u.Id == favoriteLink.UserId.Value, token)
            ?? throw new InvalidOperationException($"User with id {favoriteLink.UserId.Value} doesn't exist.");

        var book = await Context.Books
            .SingleOrDefaultAsync(b => b.Id == favoriteLink.BookId.Value, token)
            ?? throw new InvalidOperationException($"Book with id {favoriteLink.BookId.Value} doesn't exist.");

        var userFavorite = await Context.FavoriteLinks
            .SingleOrDefaultAsync(
                u => u.UserId == user.Id && u.BookId == book.Id,
                token);

        if (userFavorite != null)
        {
            throw new InvalidOperationException(
                $"User favorite with bookId {favoriteLink.BookId.Value} already exists.");
        }

        Context.FavoriteLinks.Add(new()
        {
            UserId = favoriteLink.UserId.Value,
            User = user,
            BookId = favoriteLink.BookId.Value,
            Book = book,
        });
    }

    public async Task<UsersFavorite> GetUsersFavoriteAsync(Id<User> userId, PagePagination pagePagination, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(pagePagination);

        var user = await Context.Users
            .SingleOrDefaultAsync(u => u.Id == userId.Value, token)
            ?? throw new InvalidOperationException($"User with id {userId.Value} doesn't exist.");

        var storageFavLinks = await Context.FavoriteLinks
            .OrderBy(f => f.BookId)
            .Where(f => f.UserId == user.Id)
            .Skip(pagePagination.PageSize * (pagePagination.PageNumber - 1))
            .Take(pagePagination.PageSize)
            .ToListAsync(token);

        var favoriteLinks = storageFavLinks
            .Select(f => new UserFavoriteBookLink(new(f.UserId), new(f.BookId)))
            .ToHashSet();

        return new UsersFavorite(new(userId.Value), favoriteLinks);
    }

    public async Task RemoveFavoriteLinkAsync(UserFavoriteBookLink favoriteLink, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(favoriteLink);

        var userFavorite = await Context.FavoriteLinks
            .FindAsync([favoriteLink.UserId.Value, favoriteLink.BookId.Value], token)
            ?? throw new InvalidOperationException(
                $"User favorite with bookId {favoriteLink.BookId.Value} doesn't exists.");

        Context.FavoriteLinks.Remove(userFavorite);
    }

    public async Task<long> GetTotalCountFavoriteLinkAsync(CancellationToken token) =>
        await Context.FavoriteLinks.LongCountAsync(token);
}
