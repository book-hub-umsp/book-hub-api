using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Responces;

/// <summary>
/// Транспортная модель ответа на запрос с пагинацией для книг.
/// </summary>
public sealed class GetPaginedBooksResponse : GetBooksResponse
{
    [JsonProperty("elements_total", Required = Required.Always)]
    public required long ElementsTotal { get; init; }

    [JsonProperty("pages_total", Required = Required.Always)]
    public required int PagesTotal { get; init; }
}