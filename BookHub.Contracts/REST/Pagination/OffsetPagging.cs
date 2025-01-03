﻿using System.ComponentModel.DataAnnotations;

using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Pagination;

public sealed class OffsetPagging : PaggingBase
{
    [Required]
    [JsonProperty("offset")]
    public required long Offset { get; init; }
}
