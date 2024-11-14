using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Responces.Pagination;

public sealed class OffsetPagination
{
    [Required]
    [JsonProperty("offset")]
    public required long Offset { get; init; }

    public static OffsetPagination FromDomain(Models.API.Pagination.OffsetPagination offsetPagination) =>
        new()
        {
            Offset = offsetPagination.Offset,
        };
}
