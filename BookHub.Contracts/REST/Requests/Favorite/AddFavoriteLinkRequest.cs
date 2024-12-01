using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Requests.Favorite;

/// <summary>
/// Транспортная модель запроса на добавление книги в избранное.
/// </summary>
public sealed class AddFavoriteLinkRequest
{
    [Required]
    [JsonProperty("book_id", Required = Required.Always)]
    public required long BookId { get; init; }
}