using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Responses.Favorite;

/// <summary>
/// Модель получения книг из избранного пользователя.
/// </summary>
public sealed class GetUserFavoriteResponse
{
    [Required]
    [JsonProperty("books_ids", Required = Required.Always)]
    public required IReadOnlyCollection<long> BookIds { get; init; } = [];
}