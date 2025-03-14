using System.ComponentModel.DataAnnotations;

using BookHub.API.Models.Books.Content;
using BookHub.API.Models.DomainEvents.Books;

using Newtonsoft.Json;

namespace BookHub.API.Contracts.REST.Requests.Books.Content;

/// <summary>
/// Модель обновления контента раздела книги.
/// </summary>
public sealed class UpdatePartitionContentRequest : 
    IRequestModel<PartitionUpdated<PartitionContent>>
{
    [Required]
    [JsonProperty("book_id", Required = Required.Always)]
    public required long BookId { get; init; }

    [Required]
    [JsonProperty("partition_number", Required = Required.Always)]
    public required int PartitionNumber { get; init; }

    [Required]
    [JsonProperty("new_content", Required = Required.Always)]
    public required string NewContent { get; init; }

    public PartitionUpdated<PartitionContent> ToDomain() =>
        new(new(new(BookId), new(PartitionNumber)), new(NewContent));
}