using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Requests;

/// <summary>
/// Параметры команды добавления новой книги.
/// </summary>
public abstract class AddBookParams : BookParamsBase
{
    [JsonProperty("genre", Required = Required.Always)]

    public required string Genre { get; init; }

    [JsonProperty("title", Required = Required.Always)]
    public required string Title { get; init; }

    [JsonProperty("annotation", Required = Required.Always)]
    public required string Annotation { get; init; }

    [JsonProperty("keywords", Required = Required.Always)]
    public IReadOnlyCollection<KeyWord>? Keywords { get; init; }
}