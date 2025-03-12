using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace BookHub.API.Contracts.REST.Pagination;

public sealed class PagePagging : PaggingBase
{
    [Required]
    [JsonProperty("page_number", Required = Required.Always)]
    public required int PageNumber { get; init; }
}
