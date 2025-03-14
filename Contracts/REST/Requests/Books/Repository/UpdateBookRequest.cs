using BookHub.API.Models;
using BookHub.API.Models.Books.Repository;
using BookHub.API.Models.DomainEvents.Books;
using BookHub.API.Models.Identifiers;

using Newtonsoft.Json;

namespace BookHub.API.Contracts.REST.Requests.Books.Repository;

/// <summary>
/// Параметры запроса по обновлению параметров книги.
/// </summary>
public sealed class UpdateBookRequest : IRequestModel<BookUpdatedBase>
{
    [JsonProperty("book_id", Required = Required.Always)]
    public required long BookId { get; init; }

    [JsonProperty("new_genre_id")]
    public long? NewGenreId { get; init; }

    [JsonProperty("new_status")]
    public BookStatus? NewStatus { get; init; }

    [JsonProperty("new_title")]
    public string? NewTitle { get; init; }

    [JsonProperty("new_annotation")]
    public string? NewAnnotation { get; init; }

    [JsonProperty("new_keyword_ids")]
    public IReadOnlyCollection<long>? NewKeywords { get; init; }

    public BookUpdatedBase ToDomain() =>
        (this) switch
        {
            _ when NewGenreId is not null =>
                new BookUpdated<Id<BookGenre>>(new(BookId), new(NewGenreId.Value)),

            _ when NewStatus is not null =>
                new BookUpdated<BookStatus>(new(BookId), NewStatus.Value),

            _ when NewTitle is not null =>
                new BookUpdated<Name<Book>>(new(BookId), new(NewTitle)),

            _ when NewAnnotation is not null =>
                new BookUpdated<BookAnnotation>(new(BookId), new(NewAnnotation)),

            _ when NewKeywords is not null =>
                new BookUpdated<IReadOnlyCollection<Id<KeyWord>>>(
                    new(BookId), 
                    NewKeywords.Select(x => new Id<KeyWord>(x)).ToList()),

            _ => throw new InvalidOperationException("Unreachable case.")
        };
}