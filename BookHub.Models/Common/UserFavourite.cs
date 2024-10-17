using BookHub.Models.Books;
using BookHub.Models.Users;

using System;
using System.Collections.Generic;
using System.Linq;

namespace BookHub.Models.Common;

/// <summary>
/// Избранное из книг у пользователя.
/// </summary>
public sealed class UserFavourite
{
    public Id<User> UserId { get; }

    public HashSet<UserFavouriteBookLink> Links { get; private set; } = [];

    public UserFavourite(
        Id<User> userId, 
        IReadOnlySet<UserFavouriteBookLink> links)
    {
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));

        AddLinks(userId, links);
    }

    public void ChangeLinks(IReadOnlySet<UserFavouriteBookLink> links)
    {
        AddLinks(UserId, links);
    }

    public void RemoveLinks(IReadOnlySet<Id<Book>> booksIds)
    {
        ArgumentNullException.ThrowIfNull(booksIds);

        foreach (var bookId in booksIds)
        {
            var linkToRemove = new UserFavouriteBookLink(UserId, bookId);

            if (Links.Contains(linkToRemove))
            {
                Links.Remove(linkToRemove);
            }
        }
    }

    private void AddLinks(
        Id<User> userId,
        IReadOnlySet<UserFavouriteBookLink> links)
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
