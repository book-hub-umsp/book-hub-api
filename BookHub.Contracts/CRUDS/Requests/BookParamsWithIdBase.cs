using Newtonsoft.Json;

namespace BookHub.Contracts.CRUDS.Requests;

/// <summary>
/// Базовые параметры запроса для книги.
/// </summary>
public abstract class BookParamsWithIdBase : BookParamsBase
{
    [JsonProperty("book_id", Required = Required.Always)]
    public required long BookId { get; init; }
}