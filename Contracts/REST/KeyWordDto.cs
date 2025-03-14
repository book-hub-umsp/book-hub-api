using BookHub.API.Models.Books.Repository;

using Newtonsoft.Json;

namespace BookHub.API.Contracts.REST;

/// <summary>
/// Транспортная модель ключевого слова.
/// </summary>
public sealed class KeyWordDto
{
    [JsonProperty("content", Required = Required.Always)]
    public required string Content { get; init; }

    public static KeyWordDto FromDomain(KeyWord keyWord) =>
        new()
        {
            Content = keyWord.Content.Value
        };
}