﻿using System.ComponentModel.DataAnnotations;

using BookHub.Contracts.REST.Responses.Pagination;

using Newtonsoft.Json;

using DomainModels = BookHub.Models.API;

namespace BookHub.Contracts.REST.Responses;

public sealed class NewsItemsResponse<TResponse>
{
    [JsonProperty("pagination", NullValueHandling = NullValueHandling.Ignore)]
    public PaginationBase? Pagination { get; init; }

    [Required]
    [JsonProperty("items", Required = Required.Always)]
    public required IReadOnlyCollection<TResponse> Items { get; init; }

    public static NewsItemsResponse<TResponse> FromDomain<TDomain>(
        DomainModels.NewsItems<TDomain> newsItems,
        Func<TDomain, TResponse> mapDelegate)
    {
        ArgumentNullException.ThrowIfNull(newsItems);
        ArgumentNullException.ThrowIfNull(mapDelegate);

        return new()
        {
            Items = newsItems.Select(mapDelegate).ToList(),
            Pagination = PaginationBase.FromDomain(newsItems.Pagination),
        };
    }
}