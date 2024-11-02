using BookHub.Models.Books;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BookHub.Contracts.REST.Responces;

/// <summary>
/// Транспортная модель контента книги.
/// </summary>
public sealed class BookDescriptionResponse
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

    [JsonProperty("creation_date", Required = Required.Always)]
    public required DateTimeOffset CreationDate { get; init; }

    [JsonProperty("last_edit_time", Required = Required.Always)]
    public required DateTimeOffset LastEditTime { get; init; }
}