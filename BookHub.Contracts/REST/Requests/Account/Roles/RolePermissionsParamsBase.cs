using BookHub.Models.Account;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Requests.Account.Roles;

/// <summary>
/// Базовый класс для параметров роли и его permissions.
/// </summary>
public abstract class RolePermissionsParamsBase : RoleParamsBase
{
    [JsonProperty("permissions", Required = Required.Always)]
    [JsonConverter(typeof(StringEnumConverter), typeof(SnakeCaseNamingStrategy))]
    public required IReadOnlyCollection<PermissionType> Permissions { get; init; }
}