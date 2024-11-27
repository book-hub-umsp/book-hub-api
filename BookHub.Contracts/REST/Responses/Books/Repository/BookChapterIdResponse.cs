using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Responses.Books.Repository;

/// <summary>
/// Транспортная модель идентификатора главы книги.
/// </summary>
public sealed class BookChapterIdResponse
{
    [JsonProperty("sequence_number", Required = Required.Always)]
    public required long ChapterSequenceNumber { get; init; }

    [JsonProperty("book_chapter_number", Required = Required.Always)]
    public required int BookChapterNumber { get; init; }
}