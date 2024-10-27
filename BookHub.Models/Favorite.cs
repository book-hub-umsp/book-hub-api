using BookHub.Models.Users;
using System.Collections.Generic;
using System;
using System.Linq;
using BookHub.Models.Books;

namespace BookHub.Models;

/// <summary>
/// Избранное из книг у пользователя.
/// </summary>
public sealed class Favorite
{
    public Id<User> UserId { get; }

    public HashSet<UserFavoriteBookLink> Links { get; private set; } = [];

    public Favorite(
        Id<User> userId,
        IReadOnlySet<UserFavoriteBookLink> links)
    {
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));

        AddLinks(userId, links);
    }

    public void ChangeLinks(IReadOnlySet<UserFavoriteBookLink> links) => AddLinks(UserId, links);

    public void RemoveLinks(IReadOnlySet<Id<Book>> booksIds)
    {
        ArgumentNullException.ThrowIfNull(booksIds);

        foreach (var bookId in booksIds)
        {
            var linkToRemove = new UserFavoriteBookLink(UserId, bookId);

            if (Links.Contains(linkToRemove))
            {
                Links.Remove(linkToRemove);
            }
        }
    }

    private void AddLinks(
        Id<User> userId,
        IReadOnlySet<UserFavoriteBookLink> links)
    {
        ArgumentNullException.ThrowIfNull(links);

        try
        {
            var linksDistinctByUser = links
                .DistinctBy(l => l.UserId)
                .Single(l => l.UserId == userId);
        }
        catch (InvalidOperationException)
        {
            throw new InvalidOperationException(
                $"All links should be related only for user {userId}.");
        }

        foreach (var link in links)
        {
            _ = Links.Add(link);
        }
    }
}
