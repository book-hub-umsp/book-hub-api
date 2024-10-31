using BookHub.Models.DomainEvents;
using BookHub.Models.Users;
using BookHub.Models;
using System.Net.Mail;

namespace Abstractions.Storage.Repositories;

/// <summary>
/// Описывает хранилище пользователей.
/// </summary>
public interface IUsersRepository
{
    public Task AddUserAsync(RegisteringUser user, CancellationToken token);

    public Task UpdateUserAsync(UpdatedBase<User> updated, CancellationToken token);

    public Task<UserProfileInfo> GetUserProfileInfoByEmailAsync(MailAddress mailAddress, CancellationToken token);

    public Task<UserProfileInfo> GetUserProfileInfoByIdAsync(Id<User> userId, CancellationToken token);
}
