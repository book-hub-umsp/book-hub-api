using Microsoft.AspNetCore.Authorization;

namespace BookHub.API.Service.Authentification;

/// <summary>
/// Маркер-требование для <see cref="UserExistsAuthorizationHandler"/>.
/// </summary>
public readonly record struct UserExistsRequirementMarker(bool NeedRegisterIfNotExists = false)
    : IAuthorizationRequirement;
