using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Responces;

/// <summary>
/// Транспортная модель превью книги.
/// </summary>
public sealed class BookPreview
{
    [JsonProperty("id", Required = Required.Always)]
    public required long Id { get; init; }

    [JsonProperty("author_id", Required = Required.Always)]
    public required long AuthorId { get; init; }

    [JsonProperty("title", Required = Required.Always)]
    public required string Title { get; init; }

    [JsonProperty("genre", Required = Required.Always)]
    public required string Genre { get; init; }
}