using Newtonsoft.Json;

namespace BookHub.API.Contracts.REST.Responses.Account.Roles;

/// <summary>
/// Модель ответа со списком ролей.
/// </summary>
public sealed class RolesListResponse
{
    [JsonProperty("roles", Required = Required.Always)]
    public required IReadOnlyCollection<RoleDTO> Roles { get; init; }
}
