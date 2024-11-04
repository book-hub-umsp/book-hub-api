using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Responces;

/// <summary>
/// Ответ на запрос о получении превью всех книг.
/// </summary>
public class GetAllBooksPreviewsResponse
{
    [Required]
    [JsonProperty("previews", Required = Required.Always)]
    public required IReadOnlyCollection<BookPreview> Previews { get; init; }
}