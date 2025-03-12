using Newtonsoft.Json;

namespace BookHub.API.Contracts.REST.Requests.Books.Repository;

/// <summary>
/// Базовые параметры запроса для книги.
/// </summary>
public abstract class BookParamsWithIdBase : BookParamsBase
{
    [JsonProperty("book_id", Required = Required.Always)]
    public required long BookId { get; init; }
}