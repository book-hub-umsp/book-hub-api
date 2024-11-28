using System.ComponentModel.DataAnnotations;

using BookHub.Models.API.Pagination;

using Newtonsoft.Json;

using DomainModels = BookHub.Models.API.Pagination;

namespace BookHub.Contracts.REST.Pagination;

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
            DomainModels.WithoutPagination => null,

            DomainModels.PagePagination pagePagination => new PagePagination
            {
                ItemsTotal = pagePagination.ItemsTotal,
                PagesTotal = pagePagination.PagesTotal,
                Pagging = PaggingBase.FromDomain(pagePagination.Pagging)!
            },

            DomainModels.OffsetPagination offsetPagination => new OffsetPagination
            {
                Pagging = PaggingBase.FromDomain(offsetPagination.Pagging)!
            },

            _ => throw new InvalidOperationException(
                $"Pagination {pagination.GetType().Name} is not supported.")
        };
    }
}
