using System.ComponentModel;

using BookHub.Models.Account;

using Microsoft.AspNetCore.Authorization;

namespace BookHub.API.Roles;

/// <summary>
/// Требование для <see cref="ClaimType"/>.
/// </summary>
public record struct ClaimTypeRequirement : IAuthorizationRequirement
{
    public ClaimType Claim { get; }

    public ClaimTypeRequirement(ClaimType claim)
    {
        if (!Enum.IsDefined(claim))
        {
            throw new InvalidEnumArgumentException(
                nameof(claim),
                (int)claim,
                typeof(ClaimType));
        }
    }
}