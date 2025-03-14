using BookHub.API.Abstractions.Storage.Repositories;
using BookHub.API.Models.Account;
using BookHub.API.Models.API.Pagination;
using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.Favorite;
using BookHub.API.Models.Identifiers;
using BookHub.API.Storage.PostgreSQL.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace BookHub.API.Storage.PostgreSQL.Repositories;

/// <summary>
/// Репозиторий избранного.
/// </summary>
public sealed class FavoriteLinkRepository :
    RepositoryBase,
    IFavoriteLinkRepository
{
    /// <inheritdoc/>
    public FavoriteLinkRepository(
        IRepositoryContext context)
        : base(context) { }

    /// <inheritdoc/>
    public async Task AddFavoriteLinkAsync(
        UserFavoriteBookLink favoriteLink,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(favoriteLink);

        token.ThrowIfCancellationRequested();

        var user = await Context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(
                u => u.Id == favoriteLink.UserId.Value,
                token)
            ?? throw new InvalidOperationException(
                $"User with id {favoriteLink.UserId.Value} doesn't exist.");

        var book = await Context.Books
            .AsNoTracking()
            .SingleOrDefaultAsync(
                b => b.Id == favoriteLink.BookId.Value,
                token)
            ?? throw new InvalidOperationException(
                $"Book with id {favoriteLink.BookId.Value} doesn't exist.");

        var userFavorite = await Context.FavoriteLinks
            .AsNoTracking()
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

    /// <inheritdoc/>
    public async Task<IReadOnlySet<Id<Book>>> GetUsersFavoriteBookIdsAsync(
        Id<User> userId,
        PagePagging pagePagination,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(pagePagination);

        token.ThrowIfCancellationRequested();

        var storageFavLinks = await Context.FavoriteLinks
            .AsNoTracking()
            .Where(x => x.UserId == userId.Value)
            .WithPaging(pagePagination)
            .ToListAsync(token);

        return storageFavLinks
            .Select(x => new Id<Book>(x.BookId))
            .ToHashSet();
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public async Task<long> GetTotalCountUserFavoritesLinkAsync(
        Id<User> userId,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userId);

        token.ThrowIfCancellationRequested();

        return await Context.FavoriteLinks
            .Where(x => x.UserId == userId.Value)
            .LongCountAsync(token);
    }
}