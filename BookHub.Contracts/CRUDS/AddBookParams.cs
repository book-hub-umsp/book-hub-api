using Newtonsoft.Json;

namespace BookHub.Contracts.CRUDS;

/// <summary>
/// Параметры команды добавления новой книги.
/// </summary>
public class AddBookParams
{
    [JsonProperty("genre", Required = Required.Always)]

    public required string Genre { get; init; }

    [JsonProperty("title", Required = Required.Always)]
    public required string Title { get; init; }

    [JsonProperty("annotation", Required = Required.Always)]
    public required string Annotation { get; init; }

    [JsonProperty("keywords", Required = Required.Always)]
    public IReadOnlyCollection<KeyWord> Keywords { get; set; } = null!;
}