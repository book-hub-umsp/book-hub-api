using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Responses.Books.Content;

/// <summary>
/// Модель ответа на запрос о получении контента главы.
/// </summary>
public sealed class GetChapterContentResponse
{
    [JsonProperty("book_id", Required = Required.Always)]
    public required long BookId { get; init; }

    [JsonProperty("chapter_number", Required = Required.Always)]
    public required string ChapterNumber { get; init; }

    [JsonProperty("content", Required = Required.Always)]
    public required string Content { get; init; }
}