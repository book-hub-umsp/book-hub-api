﻿using System.Net.Mail;

using BookHub.Abstractions.Storage;
using BookHub.Models;
using BookHub.Models.Account;
using BookHub.Models.API;
using BookHub.Models.API.Pagination;
using BookHub.Models.DomainEvents;

using Microsoft.Extensions.Logging;

namespace BookHub.Logic.Services.Account;

/// <summary>
/// Сервис по обработке пользователей.
/// </summary>
public sealed class UserService : IUserService
{
    private readonly IBooksHubUnitOfWork _booksHubUnitOfWork;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IBooksHubUnitOfWork booksHubUnitOfWork,
        ILogger<UserService> logger)
    {
        ArgumentNullException.ThrowIfNull(booksHubUnitOfWork);
        _booksHubUnitOfWork = booksHubUnitOfWork;

        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<UserProfileInfo?> FindUserProfileInfoByEmailAsync(MailAddress mailAddress, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(mailAddress);

        token.ThrowIfCancellationRequested();

        return await _booksHubUnitOfWork.Users.FindUserProfileInfoByEmailAsync(mailAddress, token);
    }

    /// <inheritdoc/>
    public async Task<NewsItems<UserProfileInfo>> GetUserProfilesInfoAsync(
        DataManipulation manipulation, 
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(manipulation);

        var pagePagination = new PagePagination(
            (PagePagging)manipulation.Pagination,
            await _booksHubUnitOfWork.Users.GetUsersCountAsync(token));

        var profilesInfo = await _booksHubUnitOfWork.Users.GetUserProfilesInfoAsync(
            manipulation,
            token);

        return new(profilesInfo, pagePagination);
    }

    /// <inheritdoc/>
    public async Task<UserProfileInfo> RegisterNewUserAsync(
        RegisteringUser registeringUser,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(registeringUser);

        token.ThrowIfCancellationRequested();

        var existingUser = await _booksHubUnitOfWork.Users
            .FindUserProfileInfoByEmailAsync(registeringUser.Email, token);

        if (existingUser is not null)
        {
            throw new InvalidOperationException(
                $"User with email: {existingUser!.Email.User.Take(3)}***@{existingUser.Email.Host.Take(3)}***" +
                " already exists.");
        }

        await _booksHubUnitOfWork.Users.AddUserAsync(registeringUser, token);

        await _booksHubUnitOfWork.SaveChangesAsync(token);

        existingUser = await _booksHubUnitOfWork.Users
            .FindUserProfileInfoByEmailAsync(registeringUser.Email, token);

        _logger.LogInformation(
            "New user registered with email: {EmailPattern}",
            $"{existingUser!.Email.User.Take(3)}***@{existingUser.Email.Host.Take(3)}***");

        return existingUser;
    }

    /// <inheritdoc/>
    public async Task<UserProfileInfo> GetUserProfileInfoAsync(Id<User> userId, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userId);

        return await _booksHubUnitOfWork.Users.GetUserProfileInfoByIdAsync(userId, token);
    }

    /// <inheritdoc/>
    public async Task UpdateUserInfoAsync(UpdatedBase<User> updatedUser, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(updatedUser);

        token.ThrowIfCancellationRequested();

        await _booksHubUnitOfWork.Users.UpdateUserAsync(updatedUser, token);

        await _booksHubUnitOfWork.SaveChangesAsync(token);

        _logger.LogDebug("Update user with id: {UserId} completed successfully", updatedUser.Id.Value);
    }
}