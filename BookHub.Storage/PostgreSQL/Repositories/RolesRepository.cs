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

    public async Task<Role> GetUserRoleAsync(
        Id<User> userId,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userId);

        var userRole = await Context.Users
            .AsNoTracking()
            .Where(x => x.Id == userId.Value)
            .Include(x => x.Role)
            .Select(x => x.Role)
            .SingleOrDefaultAsync(token)
                ?? throw new InvalidOperationException(
                    $"User with id '{userId.Value}' not exists");

        return new(
            new(userRole.Id),
            new(userRole.Name), 
            userRole.Permissions);
    }

    public async Task ChangeRolePermissionsAsync(
        Id<Role> roleId,
        IReadOnlySet<Permission> updatedPermissions, 
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(roleId);
        ArgumentNullException.ThrowIfNull(updatedPermissions);

        var existedRole =
            await Context.UserRoles
                .SingleOrDefaultAsync(
                    x => x.Id == roleId.Value,
                    token) 
                ?? throw new InvalidOperationException(
                    $"Role '{roleId.Value}' does not exists.");

        existedRole.Permissions = updatedPermissions.ToArray();
    }

    public async Task<IReadOnlyCollection<Role>> GetAllRolesAsync(CancellationToken token)
    {
        var storageRoles = await Context.UserRoles.ToListAsync(token);

        return storageRoles
            .Select(role => 
                new Role(
                    new(role.Id), 
                    new(role.Name), 
                    role.Permissions))
            .ToList();
    }

    public async Task ChangeUserRoleAsync(
        Id<User> userId,
        Id<Role> clarifiedRoleId,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(clarifiedRoleId);

        var storageUser = await Context.Users
            .SingleOrDefaultAsync(x => x.Id == userId.Value, token)
            ?? throw new InvalidOperationException(
                $"User with id {userId.Value} does not exists");

        var existedRole = await Context.UserRoles
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == clarifiedRoleId.Value, token)
            ?? throw new InvalidOperationException(
                $"Role '{clarifiedRoleId.Value}' does not exists.");

        storageUser.RoleId = clarifiedRoleId.Value;
    }
}