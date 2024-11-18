using BookHub.Models.Account;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Responses.Account.Permissions;

/// <summary>
/// Список из permissions, присутствующих в системе.
/// </summary>
public sealed class PermissionsListResponse
{
    [JsonProperty("permissions", Required = Required.Always)]
    [JsonConverter(typeof(StringEnumConverter), typeof(SnakeCaseNamingStrategy))]
    public required IReadOnlyCollection<PermissionType> Permissions { get; init; }
}