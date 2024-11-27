using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Pagination;

using DomainModels = Models.API.Pagination;

public abstract class PaggingBase
{
    [Required]
    [JsonProperty("page_size", Required = Required.Always)]
    public required int PageSize { get; init; }

    [Required]
    [JsonProperty("type", Required = Required.Always)]
    public required PaggingType Type { get; init; }

    public static DomainModels.PaggingBase ToDomain(PaggingBase? pagging) =>
        (pagging) switch
        {
            null => DomainModels.WithoutPagging.Instance,

            PagePagging pagePagging => new DomainModels.PagePagging(
                pagePagging.PageNumber,
                pagePagging.PageSize),

            OffsetPagging offsetPagging => new DomainModels.OffsetPagging(
                offsetPagging.Offset,
                offsetPagging.PageSize),

            _ => throw new InvalidOperationException(
                $"Unknown pagging type {pagging.GetType().Name}.")
        };

    public static PaggingBase? FromDomain(DomainModels.PaggingBase pagging) =>
        (pagging) switch
        {
            null => throw new ArgumentNullException(
                nameof(pagging),
                "Pagging can't be null."),

            DomainModels.WithoutPagging => null,

            DomainModels.PagePagging pagePagging => new PagePagging
            {
                PageNumber = pagePagging.PageNumber,
                PageSize = pagePagging.PageSize
            },

            DomainModels.OffsetPagging offsetPagging => new OffsetPagging
            {
                Offset = offsetPagging.Offset,
                PageSize = offsetPagging.PageSize
            },

            _ => throw new InvalidOperationException(
                $"Unknown pagging type {pagging.GetType().Name}.")
        };
}
