using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Responces;

/// <summary>
/// Ответ на запрос о получении книг.
/// </summary>
public class GetBooksResponse
{
    [JsonProperty("previews", Required = Required.Always)]
    public required IReadOnlyCollection<BookPreview> Previews { get; init; }
}