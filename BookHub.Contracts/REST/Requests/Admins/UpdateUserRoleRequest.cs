using BookHub.Models.Users;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Requests.Admins;

/// <summary>
/// Запрос администратора на изменение <see cref="UserRole"/>
/// для указанного пользователя.
/// </summary>
/// <remarks>
/// Даже если сменить свою роль - необходимо указывать идентификатор.
/// </remarks>
public sealed class UpdateUserRoleRequest
{
    [JsonProperty("admin_id", Required = Required.Always)]
    public required long AdminId { get; init; }

    [JsonProperty("modified_user_id", Required = Required.Always)]
    public required long ModifiedUserId { get; init; }

    [JsonProperty("new_role", Required = Required.Always)]
    [JsonConverter(typeof(StringEnumConverter), typeof(SnakeCaseNamingStrategy))]
    public required UserRole NewRole { get; init; }
}