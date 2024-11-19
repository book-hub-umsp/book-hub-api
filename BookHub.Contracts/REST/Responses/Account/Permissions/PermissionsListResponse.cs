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
    [JsonProperty(
        "permissions",
        ItemConverterType = typeof(StringEnumConverter),
        ItemConverterParameters = [typeof(SnakeCaseNamingStrategy)],        
        Required = Required.Always)]
    public required IReadOnlyCollection<Permission> Permissions { get; init; }
}