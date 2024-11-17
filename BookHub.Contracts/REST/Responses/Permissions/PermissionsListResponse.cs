using BookHub.Models.Account;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Responses.Permissions;

/// <summary>
/// Список из permissions, присутствующих в системе.
/// </summary>
public sealed class PermissionsListResponse
{
    [JsonProperty("permissions", Required = Required.Always)]
    [JsonConverter(typeof(StringEnumConverter), typeof(SnakeCaseNamingStrategy))]
    public required IReadOnlyCollection<ClaimType> Permissions { get; init; }
}