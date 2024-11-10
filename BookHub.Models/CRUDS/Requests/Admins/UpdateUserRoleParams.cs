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
    public Id<User> UserId { get; }

    public UserRole NewRole { get; }

    public UpdateUserRoleParams(
        Id<User> userId, 
        UserRole newRole)
    {
        UserId = userId ?? throw new ArgumentNullException(nameof(userId));

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