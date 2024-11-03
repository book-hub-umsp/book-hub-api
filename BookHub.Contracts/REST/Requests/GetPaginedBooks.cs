using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Requests;

/// <summary>
/// Транспортная модель запроса с пагинацией для книг.
/// </summary>
public sealed class GetPaginedBooks
{
    [JsonProperty("page_number")]
    public required int PageNumber { get; init; } = 1;

    [JsonProperty("elements_in_page")]
    public required int ElementsInPage { get; init; } = 5;
}