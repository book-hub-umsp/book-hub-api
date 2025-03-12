using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace BookHub.API.Contracts.REST.Pagination;

public sealed class PagePagination : PaginationBase
{
    [Required]
    [JsonProperty("items_total", Required = Required.Always)]
    public required long ItemsTotal { get; init; }

    [Required]
    [JsonProperty("pages_total", Required = Required.Always)]
    public required int PagesTotal { get; init; }
}
