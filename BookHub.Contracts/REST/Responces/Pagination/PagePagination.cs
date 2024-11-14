using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Responces.Pagination;

public sealed class PagePagination : PaginationBase<Models.API.Pagination.PagePagination>
{
    [Required]
    [JsonProperty("items_total", Required = Required.Always)]
    public required long ItemsTotal { get; init; }

    [Required]
    [JsonProperty("pages_total", Required = Required.Always)]
    public required int PagesTotal { get; init; }

    public override PaginationBase<Models.API.Pagination.PagePagination> FromDomain(
        Models.API.Pagination.PagePagination pagePagination) =>
        new PagePagination()
        {
            ItemsTotal = pagePagination.ItemsTotal,
            PagesTotal = pagePagination.PagesTotal,
        };
}
