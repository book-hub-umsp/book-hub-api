using System.Net.Mail;

using BookHub.API.Abstractions.Storage.Repositories;
using BookHub.API.Models;
using BookHub.API.Models.Account;
using BookHub.API.Models.API;
using BookHub.API.Models.DomainEvents.Account;
using BookHub.API.Models.Identifiers;
using BookHub.API.Storage.PostgreSQL.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace BookHub.API.Storage.PostgreSQL.Repositories;

/// <summary>
/// Хранилище пользователей.
/// </summary>
public sealed class UsersRepository :
    RepositoryBase,
    IUsersRepository
{
    private const string NOT_EXISTS_MESSAGE = "User is not exists.";

    public UsersRepository(IRepositoryContext context) : base(context) { }

    /// <inheritdoc/>
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
            RoleId = 1,
            Name = user.Name.Value,
            Email = user.Email.Address,
            Status = UserStatus.Active,
            About = "about"
        });
    }

    /// <inheritdoc/>
    public async Task UpdateUserAsync(UserUpdatedBase updated, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(updated);

        var storageUser = await Context.Users.FindAsync([updated.EntityId.Value], token)
            ?? throw new InvalidOperationException(NOT_EXISTS_MESSAGE);

        switch (updated)
        {
            case UserUpdated<Name<User>> update:
                storageUser.Name = update.Attribute.Value;
                break;

            case UserUpdated<About> update:
                storageUser.About = update.Attribute.Content;
                break;

            default:
                throw new InvalidOperationException($"Update type: {updated.GetType().Name} is not supported.");
        }
    }

    /// <inheritdoc/>
    public async Task<UserProfileInfo?> FindUserProfileInfoByEmailAsync(
        MailAddress mailAddress,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(mailAddress);

        var storageUser = await Context.Users
            .AsNoTracking()
            .Select(x => new PreviewUserInfo { Id = x.Id, Name = x.Name, Email = x.Email, About = x.About })
            .SingleOrDefaultAsync(x => x.Email == mailAddress.Address, token);

        return storageUser is not null
            ? ToUserProfileInfo(storageUser)
            : null;
    }

    /// <inheritdoc/>
    public async Task<UserProfileInfo> GetUserProfileInfoByIdAsync(
        Id<User> userId,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userId);

        var storageUser = await Context.Users
            .AsNoTracking()
            .Select(x => new PreviewUserInfo { Id = x.Id, Name = x.Name, Email = x.Email, About = x.About })
            .SingleOrDefaultAsync(x => x.Id == userId.Value, token)
            ?? throw new InvalidOperationException(NOT_EXISTS_MESSAGE);

        return ToUserProfileInfo(storageUser);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<UserProfileInfo>> GetUserProfilesInfoAsync(
        DataManipulation manipulation,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(manipulation);

        token.ThrowIfCancellationRequested();

        var storageUsers = await Context.Users
            .WithFiltering(manipulation.Filters)
            .WithPaging(manipulation.Pagination)
            .Select(x => new PreviewUserInfo { Id = x.Id, Name = x.Name, Email = x.Email, About = x.About })
            .ToListAsync(token);

        return storageUsers
            .Select(ToUserProfileInfo)
            .ToList();
    }

    /// <inheritdoc/>
    public async Task<long> GetUsersCountAsync(CancellationToken token) =>
        await Context.Users.LongCountAsync(token);

    private static UserProfileInfo ToUserProfileInfo(PreviewUserInfo userInfo) =>
        new UserProfileInfo(
            new(userInfo.Id),
            new(userInfo.Name),
            new(userInfo.Email),
            new(userInfo.About));

    private sealed class PreviewUserInfo
    {
        public required long Id { get; init; }
        public required string Name { get; init; }
        public required string Email { get; init; }
        public required string About { get; init; }
    }
}
