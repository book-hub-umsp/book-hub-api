using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BookHub.Contracts.REST.Requests;

/// <summary>
/// Параметры по удалению жанра.
/// </summary>
public sealed class RemoveGenreParams
{
    [Required]
    [JsonProperty("genre", Required = Required.Always)]
    public required string Genre { get; init; }
}
