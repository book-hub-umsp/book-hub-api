using System.Net.Mail;

using BookHub.Abstractions.Storage.Repositories;
using BookHub.Models;
using BookHub.Models.Account;
using BookHub.Storage.PostgreSQL.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace BookHub.Storage.PostgreSQL.Repositories;

/// <summary>
/// Представляет репозиторий ролей.
/// </summary>
public sealed class RolesRepository : 
    RepositoryBase, 
    IRolesRepository
{
    public RolesRepository(IRepositoryContext context) : base(context)
    {
    }

    public async Task AddRoleAsync(Role role, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(role);

        var existedRole = 
            await Context.Roles.AsNoTracking()
                .SingleOrDefaultAsync(
                    x => role.CompareTo(new(x.Name)), 
                    token);

        if (existedRole is not null)
        {
            throw new InvalidOperationException(
                $"Role with name '{role.Name}' is already exists.");
        }

        _ = Context.Roles.Add(
            new Models.Role 
            { 
                Name = role.Name.Value, 
                Claims = role.Claims.ToArray()
            });
    }

    public async Task ChangeRoleClaimsAsync(Role updatedRole, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(updatedRole);

        var existedRole =
            await Context.Roles
                .SingleOrDefaultAsync(
                    x => updatedRole.CompareTo(new(x.Name)),
                    token) 
                ?? throw new InvalidOperationException(
                    $"Role with name '{updatedRole.Name}' is not already exists.");

        existedRole.Claims = updatedRole.Claims.ToArray();
    }

    public async Task<IReadOnlyCollection<Role>> GetAllRolesAsync(CancellationToken token)
    {
        var storageRoles = await Context.Roles.ToListAsync(token);

        return storageRoles.Select(r => new Role(new(r.Name), r.Claims)).ToList();
    }

    public async Task<Role> GetUserRoleAsync(
        MailAddress mailAddress, 
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(mailAddress);

        var storageUser = await Context.Users
            .AsNoTracking()
            .Select(x => new { x.Email, x.Role })
            .SingleOrDefaultAsync(x => mailAddress.Address == x.Email, token)
                ?? throw new InvalidOperationException(
                    $"User with email '{mailAddress.Address}' not exists");

        return new(new(storageUser.Role.Name), storageUser.Role.Claims);
    }

    public async Task ChangeUserRoleAsync(
        Id<User> userId,
        Name<Role> clarifiedRoleName,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(clarifiedRoleName);

        var storageUser = await Context.Users
            .SingleOrDefaultAsync(x => x.Id == userId.Value, token)
                ?? throw new InvalidOperationException(
                    $"User with id {userId.Value} not exists");

        var role = new Role(clarifiedRoleName, []);

        var existedRole =
            await Context.Roles
                .SingleOrDefaultAsync(
                    x => role.CompareTo(new(x.Name)),
                    token)
                ?? throw new InvalidOperationException(
                    $"Role with name '{clarifiedRoleName.Value}' is not already exists.");

        storageUser.Role = existedRole;
    }
}