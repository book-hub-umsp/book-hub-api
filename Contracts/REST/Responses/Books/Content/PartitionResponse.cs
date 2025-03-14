using System.ComponentModel.DataAnnotations;

using BookHub.API.Models.Books.Content;

using Newtonsoft.Json;

namespace BookHub.API.Contracts.REST.Responses.Books.Content;

/// <summary>
/// Модель ответа на запрос о получении контента главы.
/// </summary>
public sealed class PartitionResponse : IResponseModel<PartitionResponse, Partition>
{
    [Required]
    [JsonProperty("book_id", Required = Required.Always)]
    public required long BookId { get; init; }

    [Required]
    [JsonProperty("chapter_number", Required = Required.Always)]
    public required int PartitionNumber { get; init; }

    [Required]
    [JsonProperty("content", Required = Required.Always)]
    public required string Content { get; init; }

    public static PartitionResponse FromDomain(Partition partition) =>
        new()
        {
            BookId = partition.BookId.Value,
            PartitionNumber = partition.PartitionNumber.Value,
            Content = partition.Content.Value,
        };
}