using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Responses.Books.Repository;

/// <summary>
/// Транспортная модель идентификатора главы книги.
/// </summary>
public sealed class BookChapterIdResponse
{
    [JsonProperty("chapter_id", Required = Required.Always)]
    public required long ChapterId { get; init; }

    [JsonProperty("sequence_number", Required = Required.Always)]
    public required int ChapterSequenceNumber { get; init; }
}