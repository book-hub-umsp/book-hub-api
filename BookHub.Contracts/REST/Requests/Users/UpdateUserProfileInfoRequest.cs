using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Requests.Users;

/// <summary>
/// Обновление информации о профиле пользователя.
/// </summary>
public sealed class UpdateUserProfileInfoRequest
{
    [JsonProperty("new_name")]
    public string? NewName { get; init; }

    [JsonProperty("about")]
    public string? About { get; init; }
}
