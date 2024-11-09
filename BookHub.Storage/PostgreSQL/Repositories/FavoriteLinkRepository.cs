using BookHub.Abstractions.Storage.Repositories;
using BookHub.Models;
using BookHub.Models.CRUDS.Requests;
using BookHub.Storage.PostgreSQL.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace BookHub.Storage.PostgreSQL.Repositories;

/// <summary>
/// Репозиторий избранного
/// </summary>
public sealed class FavoriteLinkRepository : RepositoryBase, IFavoriteLinkRepository
{
    public FavoriteLinkRepository(IRepositoryContext context) : base(context) { }

    public async Task AddFavoriteLink(FavoriteLinkParams favoriteLinkParams, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(favoriteLinkParams);

        var user = await Context.Users.SingleOrDefaultAsync(u => u.Id == favoriteLinkParams.UserId.Value, token)
            ?? throw new InvalidOperationException($"User with id {favoriteLinkParams.UserId.Value} doesn't exist ");

        var book = await Context.Books.SingleOrDefaultAsync(b => b.Id == favoriteLinkParams.BookId.Value, token)
            ?? throw new InvalidOperationException($"Book with id {favoriteLinkParams.BookId.Value} doesn't exist ");

        var userFavorite =
            await Context.FavoriteLinks
            .SingleOrDefaultAsync(
                u => u.UserId == user.Id && u.BookId == book.Id,
                token);

        if (userFavorite != null)
        {
            throw new InvalidOperationException(
                $"UserFavorite with bookId {favoriteLinkParams.BookId.Value} already exists");
        }

        await Context.FavoriteLinks.AddAsync(new()
        {
            UserId = favoriteLinkParams.UserId.Value,
            User = user,
            BookId = favoriteLinkParams.BookId.Value,
            Book = book,
        });

    }

    public async Task<UsersFavorite> GetUsersFavorite(UserIdParams userIdParams, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userIdParams);

        var user = await Context.Users.SingleOrDefaultAsync(u => u.Id == userIdParams.Id.Value, token)
            ?? throw new InvalidOperationException($"User with id {userIdParams.Id.Value} doesn't exist ");

        var favoriteLinks =
            Context.FavoriteLinks.Where(f => f.UserId == user.Id)
            .Select(f => new UserFavoriteBookLink(new(f.UserId), new(f.BookId)))
            .ToHashSet();

        return new UsersFavorite(new(user.Id), favoriteLinks);
    }

    public async Task RemoveFavoriteLink(FavoriteLinkParams favoriteLinkParams, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(favoriteLinkParams);

        var user = await Context.Users.SingleOrDefaultAsync(u => u.Id == favoriteLinkParams.UserId.Value, token)
            ?? throw new InvalidOperationException($"User with id {favoriteLinkParams.UserId.Value} doesn't exist ");

        var book = await Context.Books.SingleOrDefaultAsync(b => b.Id == favoriteLinkParams.BookId.Value, token)
            ?? throw new InvalidOperationException($"Book with id {favoriteLinkParams.BookId.Value} doesn't exist ");

        var userFavorite =
            await Context.FavoriteLinks
            .SingleOrDefaultAsync(
                u => u.UserId == user.Id && u.BookId == book.Id,
                token)
            ?? throw new InvalidOperationException(
                $"UserFavorite with bookId {favoriteLinkParams.BookId.Value} doesn't exists");

        Context.FavoriteLinks.Remove(userFavorite);
    }
}
