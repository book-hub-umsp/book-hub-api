using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Requests.Account.Roles;

/// <summary>
/// Класс для параметров по изменению роли пользователя.
/// </summary>
public sealed class ChangeUserRoleParams : RoleParamsBase
{
    [JsonProperty("user_id", Required = Required.Always)]
    public required long UserId { get; init; }
}