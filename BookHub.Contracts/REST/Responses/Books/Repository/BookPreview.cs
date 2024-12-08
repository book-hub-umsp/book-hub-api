using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Responses.Books.Repository;

/// <summary>
/// Транспортная модель превью книги.
/// </summary>
public sealed class BookPreview
{
    [Required]
    [JsonProperty("id", Required = Required.Always)]
    public required long Id { get; init; }

    [Required]
    [JsonProperty("author_id", Required = Required.Always)]
    public required long AuthorId { get; init; }

    [Required]
    [JsonProperty("title", Required = Required.Always)]
    public required string Title { get; init; }

    [Required]
    [JsonProperty("genre", Required = Required.Always)]
    public required string Genre { get; init; }

    [Required]
    [JsonProperty("chapters_ids", Required = Required.Always)]
    public required BookChapterIdResponse[] ChaptersNumbers { get; init; }

    public static BookPreview FromDomain(
        Models.Books.Repository.BookPreview domain)
    {
        ArgumentNullException.ThrowIfNull(domain);

        return new()
        {
            Id = domain.Id.Value,
            AuthorId = domain.AuthorId.Value,
            Genre = domain.Genre.Value,
            Title = domain.Title.Value,
            ChaptersNumbers = domain.ChaptersIdsMap
                .Select(pair => new BookChapterIdResponse
                {
                    ChapterId = pair.Key.Value,
                    ChapterSequenceNumber = pair.Value.Value,
                })
                .ToArray()
        };
    }
}