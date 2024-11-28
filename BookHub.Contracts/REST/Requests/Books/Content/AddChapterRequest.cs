using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Requests.Books.Content;

/// <summary>
/// Модель запроса на добавление главы.
/// </summary>
public sealed class AddChapterRequest
{
    [JsonProperty("book_id", Required = Required.Always)]
    public required long BookId { get; init; }

    [JsonProperty("content", Required = Required.Always)]
    public required string Content { get; init; }
}