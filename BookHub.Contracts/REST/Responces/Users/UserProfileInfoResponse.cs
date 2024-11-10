using System.ComponentModel.DataAnnotations;

using BookHub.Models.Users;

using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Responces.Users;

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

    public static UserProfileInfoResponse FromDomain(UserProfileInfo userProfileInfo) =>
        new()
        {
            Id = userProfileInfo.Id.Value,
            Name = userProfileInfo.Name.Value,
            Email = userProfileInfo.Email.Address,
            About = userProfileInfo.About.Content
        };
}
