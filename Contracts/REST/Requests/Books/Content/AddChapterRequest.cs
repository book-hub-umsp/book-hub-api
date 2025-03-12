using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace BookHub.API.Contracts.REST.Requests.Books.Content;

/// <summary>
/// Модель запроса на добавление главы.
/// </summary>
public sealed class AddChapterRequest
{
    [Required]
    [JsonProperty("book_id", Required = Required.Always)]
    public required long BookId { get; init; }

    [Required]
    [JsonProperty("content", Required = Required.Always)]
    public required string Content { get; init; }
}