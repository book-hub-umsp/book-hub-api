using Microsoft.AspNetCore.Authorization;

namespace BookHub.API.Authentification;

/// <summary>
/// Маркер-требование для <see cref="UserExistsAuthorizationHandler"/>.
/// </summary>
public class UserExistsRequirementMarker : IAuthorizationRequirement;
