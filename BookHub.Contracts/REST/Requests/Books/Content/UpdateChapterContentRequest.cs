﻿using Newtonsoft.Json;

namespace BookHub.Contracts.REST.Requests.Books.Content;

/// <summary>
/// Модель обновления контента главы книги.
/// </summary>
public sealed class UpdateChapterContentRequest
{
    [JsonProperty("chapter_id", Required = Required.Always)]
    public required long ChapterId { get; init; }

    [JsonProperty("new_content", Required = Required.Always)]
    public required string NewContent { get; init; }
}