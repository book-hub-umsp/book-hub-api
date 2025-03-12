using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace BookHub.API.Contracts.REST.Responses.Books.Content;

/// <summary>
/// Модель ответа на запрос о получении контента главы.
/// </summary>
public sealed class GetChapterContentResponse
{
    [Required]
    [JsonProperty("book_id", Required = Required.Always)]
    public required long BookId { get; init; }

    [Required]
    [JsonProperty("chapter_number", Required = Required.Always)]
    public required int ChapterNumber { get; init; }

    [Required]
    [JsonProperty("content", Required = Required.Always)]
    public required string Content { get; init; }
}