using System.Net.Mail;

using BookHub.Models;
using BookHub.Models.DomainEvents;
using BookHub.Models.Users;

namespace BookHub.Abstractions.Repositories;

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