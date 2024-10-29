using BookHub.Models.Books;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BookHub.Contracts.CRUDS.Responces;

/// <summary>
/// Транспортная модель контента книги.
/// </summary>
public sealed class BookContent
{
    [JsonProperty("author_id", Required = Required.Always)]
    public required long AuthorId { get; init; }

    [JsonProperty("genre", Required = Required.Always)]
    public required string Genre { get; init; }

    [JsonProperty("status", Required = Required.Always)]
    [JsonConverter(typeof(StringEnumConverter))]
    public required BookStatus BookStatus { get; init; }

    [JsonProperty("title", Required = Required.Always)]
    public required string Title { get; init; }

    [JsonProperty("annotation", Required = Required.Always)]
    public required string Annotation { get; init; }

    [JsonProperty("keywords", Required = Required.Always)]
    public IReadOnlyCollection<KeyWord> Keywords { get; set; } = null!;
}