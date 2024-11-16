using BookHub.Models.Account;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace BookHub.Contracts.REST.Requests.Account.Roles;

/// <summary>
/// Базовый класс для параметров роли.
/// </summary>
public abstract class RoleParamsBase
{
    [JsonProperty("name", Required = Required.Always)]
    public required string Name { get; init; }
}