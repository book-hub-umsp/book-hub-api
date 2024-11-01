using Newtonsoft.Json;

namespace BookHub.Contracts.CRUDS.Requests;

/// <summary>
/// Параметры команды добавления новой книги с автором.
/// </summary>
public sealed class AddAuthorBookParams : AddBookParams
{
    [JsonProperty("author_id", Required = Required.Always)]
    public long AuthorId { get; init; }
}