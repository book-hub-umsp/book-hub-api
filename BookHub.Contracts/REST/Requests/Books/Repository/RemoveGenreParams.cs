using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Requests.Books.Repository;

/// <summary>
/// Параметры по удалению жанра.
/// </summary>
public sealed class RemoveGenreParams
{
    [Required]
    [JsonProperty("genre", Required = Required.Always)]
    public required string Genre { get; init; }
}
