﻿using Newtonsoft.Json;

namespace BookHub.Contracts;

/// <summary>
/// Транспортная модель контейнера ключевых слов.
/// </summary>
public sealed class KeyWordsContainer
{
    [JsonProperty("keywords", Required = Required.Always)]
    public IReadOnlyCollection<KeyWord> Keywords { get; set; }
}