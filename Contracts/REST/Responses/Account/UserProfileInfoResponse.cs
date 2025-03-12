using System.ComponentModel.DataAnnotations;

using BookHub.API.Models.Account;

using Newtonsoft.Json;

namespace BookHub.API.Contracts.REST.Responses.Account;

public sealed class UserProfileInfoResponse
{
    [Required]
    [JsonProperty("id", Required = Required.Always)]
    public required long Id { get; init; }

    [Required]
    [JsonProperty("name", Required = Required.Always)]
    public required string Name { get; init; }

    [Required]
    [JsonProperty("email", Required = Required.Always)]
    public required string Email { get; init; }

    [Required]
    [JsonProperty("about", Required = Required.Always)]
    public required string About { get; init; }

    public static UserProfileInfoResponse FromDomain(UserProfileInfo userProfileInfo)
    {
        ArgumentNullException.ThrowIfNull(userProfileInfo);

        return new()
        {
            Id = userProfileInfo.Id.Value,
            Name = userProfileInfo.Name.Value,
            Email = userProfileInfo.Email.Address,
            About = userProfileInfo.About.Content
        };
    }
}
