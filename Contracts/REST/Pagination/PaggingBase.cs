using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace BookHub.API.Contracts.REST.Pagination;

using Domain = Models.API.Pagination;

public abstract class PaggingBase
{
    [Required]
    [JsonProperty("page_size", Required = Required.Always)]
    public required int PageSize { get; init; }

    [Required]
    [JsonProperty("type", Required = Required.Always)]
    public required PaggingType Type { get; init; }

    public static Domain.PaggingBase ToDomain(PaggingBase? pagging) =>
        pagging switch
        {
            null => Domain.WithoutPagging.Instance,

            PagePagging pagePagging => new Domain.PagePagging(
                pagePagging.PageNumber,
                pagePagging.PageSize),

            OffsetPagging offsetPagging => new Domain.OffsetPagging(
                offsetPagging.Offset,
                offsetPagging.PageSize),

            _ => throw new InvalidOperationException(
                $"Unknown pagging type {pagging.GetType().Name}.")
        };

    public static PaggingBase? FromDomain(Domain.PaggingBase pagging) =>
        pagging switch
        {
            null => throw new ArgumentNullException(
                nameof(pagging),
                "Pagging can't be null."),

            Domain.WithoutPagging => null,

            Domain.PagePagging pagePagging => new PagePagging
            {
                Type = PaggingType.Page,
                PageNumber = pagePagging.PageNumber,
                PageSize = pagePagging.PageSize
            },

            Domain.OffsetPagging offsetPagging => new OffsetPagging
            {
                Type = PaggingType.Offset,
                Offset = offsetPagging.Offset,
                PageSize = offsetPagging.PageSize
            },

            _ => throw new InvalidOperationException(
                $"Unknown pagging type {pagging.GetType().Name}.")
        };
}
