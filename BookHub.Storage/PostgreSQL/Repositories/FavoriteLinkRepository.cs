using BookHub.Abstractions.Storage.Repositories;
using BookHub.Models;
using BookHub.Models.Users;
using BookHub.Storage.PostgreSQL.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace BookHub.Storage.PostgreSQL.Repositories;

/// <summary>
/// Репозиторий избранного.
/// </summary>
public sealed class FavoriteLinkRepository : RepositoryBase, IFavoriteLinkRepository
{
    public FavoriteLinkRepository(IRepositoryContext context) : base(context) { }

    public async Task AddFavoriteLink(UserFavoriteBookLink favoriteLinkParams, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(favoriteLinkParams);

        var user = await Context.Users
            .SingleOrDefaultAsync(u => u.Id == favoriteLinkParams.UserId.Value, token)
            ?? throw new InvalidOperationException($"User with id {favoriteLinkParams.UserId.Value} doesn't exist.");

        var book = await Context.Books
            .SingleOrDefaultAsync(b => b.Id == favoriteLinkParams.BookId.Value, token)
            ?? throw new InvalidOperationException($"Book with id {favoriteLinkParams.BookId.Value} doesn't exist.");

        var userFavorite = await Context.FavoriteLinks
            .SingleOrDefaultAsync(
                u => u.UserId == user.Id && u.BookId == book.Id,
                token);

        if (userFavorite != null)
        {
            throw new InvalidOperationException(
                $"User favorite with bookId {favoriteLinkParams.BookId.Value} already exists.");
        }

        Context.FavoriteLinks.Add(new()
        {
            UserId = favoriteLinkParams.UserId.Value,
            User = user,
            BookId = favoriteLinkParams.BookId.Value,
            Book = book,
        });
    }

    public async Task<UsersFavorite> GetUsersFavorite(Id<User> userId, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userId);

        var user = await Context.Users
            .SingleOrDefaultAsync(u => u.Id == userId.Value, token)
            ?? throw new InvalidOperationException($"User with id {userId.Value} doesn't exist.");

        var storageFavLinks = await Context.FavoriteLinks
            .Where(f => f.UserId == user.Id)
            .ToListAsync();

        var favoriteLinks = storageFavLinks
            .Select(f => new UserFavoriteBookLink(new(f.UserId), new(f.BookId)))
            .ToHashSet();


        return new UsersFavorite(new(userId.Value), favoriteLinks);
    }

    public async Task RemoveFavoriteLink(UserFavoriteBookLink favoriteLinkParams, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(favoriteLinkParams);

        var userFavorite = await Context.FavoriteLinks
            .FindAsync([favoriteLinkParams.UserId.Value, favoriteLinkParams.BookId.Value], token)
            ?? throw new InvalidOperationException(
                $"User favorite with bookId {favoriteLinkParams.BookId.Value} doesn't exists.");

        Context.FavoriteLinks.Remove(userFavorite);
    }
}
