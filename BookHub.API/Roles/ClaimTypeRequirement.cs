using BookHub.Models.Account;
using Microsoft.AspNetCore.Authorization;

namespace BookHub.API.Roles;

/// <summary>
/// Требование для <see cref="ClaimType"/>.
/// </summary>
public sealed class ClaimTypeRequirement : IAuthorizationRequirement
{
    public ClaimType ClaimType { get; }

    public ClaimTypeRequirement(ClaimType claimType)
    {
        ClaimType = claimType;
    }
}