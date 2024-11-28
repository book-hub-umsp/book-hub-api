using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Responses.Books.Repository;

/// <summary>
/// Транспортная модель идентификатора главы книги.
/// </summary>
public sealed class BookChapterIdResponse
{
    [Required]
    [JsonProperty("chapter_id", Required = Required.Always)]
    public required long ChapterId { get; init; }

    [Required]
    [JsonProperty("sequence_number", Required = Required.Always)]
    public required int ChapterSequenceNumber { get; init; }
}