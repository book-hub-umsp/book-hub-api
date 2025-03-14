using System.ComponentModel.DataAnnotations;

using BookHub.API.Models.Books.Content;

using Newtonsoft.Json;

namespace BookHub.API.Contracts.REST.Requests.Books.Content;

/// <summary>
/// Модель запроса на добавление раздела.
/// </summary>
public sealed class AddPartitionRequest : IRequestModel<CreatingPartition>
{
    [Required]
    [JsonProperty("book_id", Required = Required.Always)]
    public required long BookId { get; init; }

    [Required]
    [JsonProperty("content", Required = Required.Always)]
    public required string Content { get; init; }

    public CreatingPartition ToDomain() => new(new(BookId), new(Content));
}