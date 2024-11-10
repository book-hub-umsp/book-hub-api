using System.Net.Mail;

using BookHub.Abstractions.Storage.Repositories;
using BookHub.Models;
using BookHub.Models.CRUDS.Requests.Admins;
using BookHub.Models.DomainEvents;
using BookHub.Models.DomainEvents.Users;
using BookHub.Models.Users;
using BookHub.Storage.PostgreSQL.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace BookHub.Storage.PostgreSQL.Repositories;

/// <summary>
/// Хранилище пользователей.
/// </summary>
public sealed class UsersRepository :
    RepositoryBase,
    IUsersRepository
{
    private const string NOT_EXISTS_MESSAGE = "User is not exists.";

    public UsersRepository(IRepositoryContext context) : base(context) { }

    public async Task AddUserAsync(RegisteringUser user, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(user);

        var existingEmail = await Context.Users
            .AsNoTracking()
            .Select(x => x.Email)
            .SingleOrDefaultAsync(email => email == user.Email.Address, token);

        if (existingEmail is not null)
        {
            throw new InvalidOperationException("User with such email is alredy exists.");
        }

        Context.Users.Add(new()
        {
            Name = user.Name.Value,
            Email = user.Email.Address,
            Status = UserStatus.Active,
            Role = UserRole.Default,
            About = "about"
        });
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

    public async Task<UserProfileInfo?> FindUserProfileInfoByEmailAsync(
        MailAddress mailAddress,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(mailAddress);

        var storageUser = await Context.Users
            .AsNoTracking()
            .Select(x => new { x.Id, x.Name, x.Email, x.About, x.Role })
            .SingleOrDefaultAsync(x => x.Email == mailAddress.Address, token);

        return storageUser is not null
            ? new UserProfileInfo(
                new(storageUser.Id),
                new(storageUser.Name),
                new(storageUser.Email),
                new(storageUser.About),
                storageUser.Role)
            : null;
    }

    public async Task<UserProfileInfo> GetUserProfileInfoByIdAsync(
        Id<User> userId,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userId);

        var storageUser = await Context.Users
            .AsNoTracking()
            .Select(x => new { x.Id, x.Name, x.Email, x.About, x.Role })
            .SingleOrDefaultAsync(x => x.Id == userId.Value, token)
            ?? throw new InvalidOperationException(NOT_EXISTS_MESSAGE);

        return new UserProfileInfo(
            new(storageUser.Id),
            new(storageUser.Name),
            new(storageUser.Email),
            new(storageUser.About),
            storageUser.Role);
    }

    public async Task UpdateUserRoleAsync(
        UpdateUserRoleParams updateUserRoleParams,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(updateUserRoleParams);

        var storageUser = await Context.Users
            .SingleOrDefaultAsync(x => x.Id == updateUserRoleParams.ModifiedUserId.Value, token)
            ?? throw new InvalidOperationException(NOT_EXISTS_MESSAGE);

        storageUser.Role = updateUserRoleParams.NewRole;
    }

    public async Task<bool> HasModeratorOptions(Id<User> userId, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userId);

        var storageUser = await Context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == userId.Value, token)
            ?? throw new InvalidOperationException(NOT_EXISTS_MESSAGE);

        return storageUser.Role == UserRole.Moderator;
    }

    public async Task<bool> HasAdminOptions(Id<User> userId, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userId);

        var storageUser = await Context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == userId.Value, token)
            ?? throw new InvalidOperationException(NOT_EXISTS_MESSAGE);

        return storageUser.Role == UserRole.Admin;
    }
}