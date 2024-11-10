using System;
using System.ComponentModel;
using System.Data;

using BookHub.Models.Users;

namespace BookHub.Models.CRUDS.Requests.Admins;

/// <summary>
/// Модель с параметрами для обновления роли пользователя.
/// </summary>
public sealed class UpdateUserRoleParams
{
    public Id<User> AdminId { get; init; }

    public Id<User> ModifiedUserId { get; }

    public UserRole NewRole { get; }

    public UpdateUserRoleParams(
        Id<User> adminId,
        Id<User> modifiedUserId,    
        UserRole newRole)
    {
        AdminId = adminId ?? throw new ArgumentNullException(nameof(adminId));
        ModifiedUserId = modifiedUserId 
            ?? throw new ArgumentNullException(nameof(modifiedUserId));

        if (!Enum.IsDefined(newRole))
        {
            throw new InvalidEnumArgumentException(
                nameof(newRole),
                (int)newRole,
                typeof(UserRole));
        }
        NewRole = newRole;
    }
}