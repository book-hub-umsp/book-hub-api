﻿using Newtonsoft.Json;

namespace BookHub.Contracts.CRUDS;

/// <summary>
/// Параметры запроса по обновлению параметров книги.
/// </summary>
public sealed class UpdateBookParams
{
    [JsonProperty("genre", NullValueHandling = NullValueHandling.Ignore)]

    public string? NewGenre { get; init; } = null!;

    [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]

    public string? NewStatus { get; init; } = null!;


    [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
    public string? NewTitle { get; init; } = null!;


    [JsonProperty("annotation", NullValueHandling = NullValueHandling.Ignore)]
    public string? NewAnnotation { get; init; }


    [JsonProperty("keywords", Required = Required.Always)]
    public IReadOnlyCollection<KeyWord> UpdatedKeywords { get; set; } = null!;
}