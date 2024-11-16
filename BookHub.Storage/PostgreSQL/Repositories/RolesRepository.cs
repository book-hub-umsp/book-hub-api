using BookHub.Abstractions.Storage.Repositories;
using BookHub.Models.Account;
using BookHub.Storage.PostgreSQL.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace BookHub.Storage.PostgreSQL.Repositories;

/// <summary>
/// Представляет репозиторий ролей.
/// </summary>
public sealed class RolesRepository : RepositoryBase, IRolesRepository
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
}