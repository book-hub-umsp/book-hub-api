using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace BookHub.API.Contracts.REST.Pagination;

public sealed class OffsetPagging : PaggingBase
{
    [Required]
    [JsonProperty("offset")]
    public required long Offset { get; init; }
}
