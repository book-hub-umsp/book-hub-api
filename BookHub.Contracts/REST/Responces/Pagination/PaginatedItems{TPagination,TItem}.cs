using System.ComponentModel.DataAnnotations;

using BookHub.Models.API.Pagination;

using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Responces.Pagination;

public sealed class PaginatedItems<TResponsePagination, TResponseItem>
    where TResponsePagination : PaginationBase
{
    [Required]
    [JsonProperty("pagination")]
    public required TResponsePagination Pagination { get; init; }

    [Required]
    [JsonProperty("items")]
    public required IReadOnlyCollection<TResponseItem> Items { get; init; }

    public static PaginatedItems<TResponsePagination, TResponseItem> FromDomain<TPagination, TItem>(
        PaginatedItems<TPagination, TItem> paginationItems)
        where TPagination : PaginationBase
        => 
        new()
        {
            Pagination = paginationItems.Pagination.
        };
}
