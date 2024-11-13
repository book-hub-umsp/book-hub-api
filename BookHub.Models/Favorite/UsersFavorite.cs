using System;
using System.Collections.Generic;

using BookHub.Models.Account;

namespace BookHub.Models.Favorite;

/// <summary>
/// Избранное из книг у пользователя.
/// </summary>
public sealed class UsersFavorite
{
    public Id<User> UserId { get; }
    public HashSet<UserFavoriteBookLink> Links { get; private set; } = [];

    public UsersFavorite(
        Id<User> userId,
        IReadOnlySet<UserFavoriteBookLink> links)
    {
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));

        ArgumentNullException.ThrowIfNull(links);
        Links.UnionWith(links);
    }
}
