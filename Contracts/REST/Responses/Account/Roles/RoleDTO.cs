using BookHub.API.Models.Account;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace BookHub.API.Contracts.REST.Responses.Account.Roles;

/// <summary>
/// Транспортная модель роли.
/// </summary>
public sealed class RoleDTO
{
    [JsonProperty("id", Required = Required.Always)]
    public required long RoleId { get; init; }

    [JsonProperty("name", Required = Required.Always)]
    public required string Name { get; init; }

    [JsonProperty(
        "permissions",
        ItemConverterType = typeof(StringEnumConverter),
        ItemConverterParameters = [typeof(SnakeCaseNamingStrategy)],
        Required = Required.Always)]
    public required IReadOnlyCollection<Permission> Permissions { get; init; }

    public static RoleDTO FromDomain(Role role) =>
        new()
        {
            RoleId = role.Id.Value,
            Name = role.Name.Value,
            Permissions = role.Permissions,
        };
}