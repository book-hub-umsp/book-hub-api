using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Responses.Pagination;

public sealed class OffsetPagination : PaginationBase
{
    [Required]
    [JsonProperty("offset")]
    public required long Offset { get; init; }
}
