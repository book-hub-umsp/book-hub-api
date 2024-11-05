using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Responces;

/// <summary>
/// Ответ на запрос о получении всех жанров.
/// </summary>
public sealed class BooksGenresListResponse
{
    [Required]
    [JsonProperty("books_genres", Required = Required.Always)]
    public required IReadOnlyCollection<string> BookGenres { get; init; }
}