using System.ComponentModel.DataAnnotations;

using BookHub.API.Models.API.Pagination;

using Newtonsoft.Json;

using Domain = BookHub.API.Models.API.Pagination;

namespace BookHub.API.Contracts.REST.Pagination;

public abstract class PaginationBase
{
    [Required]
    [JsonProperty("pagging", Required = Required.Always)]
    public required PaggingBase Pagging { get; init; }

    public static PaginationBase? FromDomain(IPagination pagination)
    {
        ArgumentNullException.ThrowIfNull(pagination);

        return pagination switch
        {
            WithoutPagination => null,

            Domain.PagePagination pagePagination => new PagePagination
            {
                ItemsTotal = pagePagination.ItemsTotal,
                PagesTotal = pagePagination.PagesTotal,
                Pagging = PaggingBase.FromDomain(pagePagination.Pagging)!
            },

            Domain.OffsetPagination offsetPagination => new OffsetPagination
            {
                Pagging = PaggingBase.FromDomain(offsetPagination.Pagging)!
            },

            _ => throw new InvalidOperationException(
                $"Pagination {pagination.GetType().Name} is not supported.")
        };
    }
}
