using System.Net.Mail;

using BookHub.Abstractions.Repositories;
using BookHub.Models;
using BookHub.Models.DomainEvents;
using BookHub.Models.DomainEvents.Users;
using BookHub.Models.Users;
using BookHub.Storage.PostgreSQL.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace BookHub.Storage.PostgreSQL.Repositories;

/// <summary>
/// Хранилище пользователей.
/// </summary>
public sealed class UsersRepository : RepositoryBase, IUsersRepository
{
    private const string NOT_EXISTS_MESSAGE = "User is not exists.";

    public UsersRepository(IRepositoryContext context) : base(context) { }

    public Task AddUserAsync(RegisteringUser user, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(user);

        Context.Users.Add(new()
        {
            Name = user.Name.Value,
            Email = user.Email.Address,
            Status = UserStatus.Active,
            Permission = UserPermission.None
        });

        return Task.CompletedTask;
    }

    public async Task UpdateUserAsync(UpdatedBase<User> updated, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(updated);

        var storageUser = await Context.Users.FindAsync([updated.Id.Value], token)
            ?? throw new InvalidOperationException(NOT_EXISTS_MESSAGE);

        switch (updated)
        {
            case Updated<Name<User>> update:
                storageUser.Name = update.Attribute.Value;
                break;

            case Updated<About> update:
                storageUser.About = update.Attribute.Content;
                break;

            default:
                throw new InvalidOperationException($"Update type: {updated.GetType().Name} is not supported.");
        }
    }

    public async Task<UserProfileInfo> GetUserProfileInfoByIdAsync(
        Id<User> userId,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userId);

        var storageUser = await Context.Users
            .AsNoTracking()
            .Select(x => new { x.Id, x.Name, x.Email, x.About })
            .SingleOrDefaultAsync(x => x.Id == userId.Value, token)
            ?? throw new InvalidOperationException(NOT_EXISTS_MESSAGE);

        return new UserProfileInfo(
            new(storageUser.Id),
            new(storageUser.Name),
            new(storageUser.Email),
            new(storageUser.About));
    }

    public async Task<UserProfileInfo> GetUserProfileInfoByEmailAsync(
        MailAddress mailAddress,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(mailAddress);

        var storageUser = await Context.Users
            .AsNoTracking()
            .Select(x => new { x.Id, x.Name, x.Email, x.About })
            .SingleOrDefaultAsync(x => x.Email == mailAddress.Address, token)
            ?? throw new InvalidOperationException(NOT_EXISTS_MESSAGE);

        return new UserProfileInfo(
            new(storageUser.Id),
            new(storageUser.Name),
            new(storageUser.Email),
            new(storageUser.About));
    }
}
