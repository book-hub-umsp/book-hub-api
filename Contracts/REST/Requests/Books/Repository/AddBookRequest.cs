using BookHub.API.Models.Books;
using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.Identifiers;

using Newtonsoft.Json;

namespace BookHub.API.Contracts.REST.Requests.Books.Repository;

/// <summary>
/// Параметры команды добавления новой книги.
/// </summary>
public sealed class AddBookRequest
{
    [JsonProperty("genre_id", Required = Required.Always)]
    public required long GenreId { get; init; }

    [JsonProperty("title", Required = Required.Always)]
    public required string Title { get; init; }

    [JsonProperty("annotation", Required = Required.Always)]
    public required string Annotation { get; init; }

    [JsonProperty("keyword_ids")]
    public IReadOnlyCollection<long>? Keywords { get; init; }

    public CreatingBook ToDomain() =>
        new(new(GenreId),
            new(Title), 
            new(Annotation),
            Keywords?
                .Select(x => new Id<KeyWord>(x))
                .ToHashSet() 
            ?? []);
}