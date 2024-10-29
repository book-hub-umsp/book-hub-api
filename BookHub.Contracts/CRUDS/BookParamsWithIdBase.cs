﻿using Newtonsoft.Json;

namespace BookHub.Contracts.CRUDS;

/// <summary>
/// Базовые параметры запроса для книги.
/// </summary>
public abstract class BookParamsWithIdBase
{
    [JsonProperty("book_id", Required = Required.Always)]
    public required long BookId { get; init; }
}