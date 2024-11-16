using BookHub.Models.Account;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Requests.Account.Roles;

/// <summary>
/// Базовый класс для параметров роли и клэймов.
/// </summary>
public abstract class RoleClaimsParamsBase : RoleParamsBase
{
    [JsonProperty("claims", Required = Required.Always)]
    [JsonConverter(typeof(StringEnumConverter), typeof(SnakeCaseNamingStrategy))]
    public required IReadOnlyCollection<ClaimType> Claims { get; init; }
}