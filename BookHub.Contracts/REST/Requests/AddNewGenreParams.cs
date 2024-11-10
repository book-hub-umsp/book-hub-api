using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Requests;

/// <summary>
/// Параметры по добавлению нового жанра.
/// </summary>
public sealed class AddNewGenreParams
{
    [Required]
    [JsonProperty("new_genre", Required = Required.Always)]
    public required string NewGenre { get; init; }
}
