using BookHub.API.Models.Books.Repository;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BookHub.API.Contracts.REST.Responses.Books.Repository;

/// <summary>
/// Транспортная модель описания книги.
/// </summary>
public sealed class BookDescriptionResponse : 
    IResponseModel<BookDescriptionResponse, Book>
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
    public IReadOnlyCollection<KeyWordDto> Keywords { get; set; } = null!;

    [JsonProperty("creation_date", Required = Required.Always)]
    public required DateTimeOffset CreationDate { get; init; }

    [JsonProperty("last_edit_time", Required = Required.Always)]
    public required DateTimeOffset LastEditTime { get; init; }

    public static BookDescriptionResponse FromDomain(Book domain) =>
        new()
        {
            AuthorId = domain.AuthorId.Value,
            Title = domain.Description.Title.Value,
            Genre = domain.Description.Genre.Value,
            Annotation = domain.Description.BookAnnotation.Content,
            BookStatus = domain.Status,
            CreationDate = domain.CreationDate,
            LastEditTime = domain.LastEditDate,
            Keywords = domain.Description.KeyWords
                    .Select(KeyWordDto.FromDomain)
                    .ToList()
        };
}