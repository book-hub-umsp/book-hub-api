using BookHub.Models.Account;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Requests.Account.Roles;

/// <summary>
/// Модель изменения permissions для роли.
/// </summary>
public sealed class ChangeRolePermissionsParams : RoleParamsBase
{
    [JsonProperty("permissions", Required = Required.Always)]
    [JsonConverter(typeof(StringEnumConverter), typeof(SnakeCaseNamingStrategy))]
    public required IReadOnlyCollection<PermissionType> Permissions { get; init; }
}
