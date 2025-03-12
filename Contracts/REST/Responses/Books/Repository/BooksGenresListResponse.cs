using System.ComponentModel.DataAnnotations;

using BookHub.API.Models.Books.Repository;

using Newtonsoft.Json;

namespace BookHub.API.Contracts.REST.Responses.Books.Repository;

/// <summary>
/// Ответ на запрос о получении всех жанров.
/// </summary>
public sealed class BooksGenresListResponse
{
    [Required]
    [JsonProperty("books_genres", Required = Required.Always)]
    public required IReadOnlyCollection<string> BookGenres { get; init; }

    public static BooksGenresListResponse FromDomainsList(
        IReadOnlyCollection<BookGenre> booksGenres)
    {
        ArgumentNullException.ThrowIfNull(booksGenres);

        return new()
        {
            BookGenres = booksGenres
                .Select(x => SnakeCaseToPascalCase(x.Value))
                .ToList()
        };
    }

    private static string SnakeCaseToPascalCase(string str) =>
        Thread.CurrentThread.CurrentCulture.TextInfo
            .ToTitleCase(str.Replace('_', ' '))
            .Replace(" ", string.Empty);
}