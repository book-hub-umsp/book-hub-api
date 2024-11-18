using Microsoft.AspNetCore.Authorization;

namespace BookHub.API.Authentification;

/// <summary>
/// Маркер-требование для <see cref="UserExistsAuthorizationHandler"/>.
/// </summary>
public readonly record struct UserExistsRequirementMarker(bool NeedRegisterIfNotExists = false) 
    : IAuthorizationRequirement;
