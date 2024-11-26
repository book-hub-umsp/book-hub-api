using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

using DomainModels = BookHub.Models.API.Pagination;

namespace BookHub.Contracts.REST.Pagination;

public abstract class PaginationBase
{
    [Required]
    [JsonProperty("page_size", Required = Required.Always)]
    public required int PageSize { get; init; }

    public static DomainModels.PaginationBase ToDomain(PaginationBase? pagination) => 
        (pagination) switch
        {
            null => DomainModels.WithoutPagging.Instance,

            PagePagination => new DomainModels.PagePagination()
        }

    public static PaginationBase? FromDomain(DomainModels.PaginationBase pagination)
    {
        ArgumentNullException.ThrowIfNull(pagination);

        return pagination switch
        {
            DomainModels.WithoutPagging => null,

            DomainModels.PagePagination pagePagination => new PagePagination()
            {
                ItemsTotal = pagePagination.ItemsTotal,
                PagesTotal = pagePagination.PagesTotal,
                PageNumber = pagePagination.PageNumber,
                PageSize = pagePagination.PageSize,
            },

            DomainModels.OffsetPagination offsetPagination => new OffsetPagination()
            {
                Offset = offsetPagination.Offset,
                PageSize = offsetPagination.PageSize,
            },

            _ => throw new InvalidOperationException(
                $"Pagination {pagination.GetType().Name} is not supported.")
        };
    }
}
