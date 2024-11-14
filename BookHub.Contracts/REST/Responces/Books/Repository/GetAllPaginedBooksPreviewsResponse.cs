using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Responces.Books.Repository;

/// <summary>
/// Транспортная модель ответа на запрос с пагинацией для книг.
/// </summary>
public sealed class GetAllPaginedBooksPreviewsResponse : GetAllBooksPreviewsResponse
{
    [Required]
    [JsonProperty("elements_total", Required = Required.Always)]
    public required long ElementsTotal { get; init; }

    [Required]
    [JsonProperty("pages_total", Required = Required.Always)]
    public required int PagesTotal { get; init; }
}