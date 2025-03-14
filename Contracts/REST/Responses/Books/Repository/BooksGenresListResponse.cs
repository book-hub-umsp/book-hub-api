using System.ComponentModel.DataAnnotations;

using BookHub.API.Models.Books.Repository;

using Newtonsoft.Json;

namespace BookHub.API.Contracts.REST.Responses.Books.Repository;

/// <summary>
/// Ответ на запрос о получении всех жанров.
/// </summary>
public sealed class BooksGenresListResponse :
    IResponseModel<BooksGenresListResponse, IReadOnlyCollection<BookGenre>>
{
    [Required]
    [JsonProperty("books_genres", Required = Required.Always)]
    public required IReadOnlyCollection<string> BookGenres { get; init; }

    public static BooksGenresListResponse FromDomain(
        IReadOnlyCollection<BookGenre> domain)
    {
        ArgumentNullException.ThrowIfNull(domain);

        return new()
        {
            BookGenres = domain
                .Select(x => SnakeCaseToPascalCase(x.Value))
                .ToList()
        };
    }

    private static string SnakeCaseToPascalCase(string str) =>
        Thread.CurrentThread.CurrentCulture.TextInfo
            .ToTitleCase(str.Replace('_', ' '))
            .Replace(" ", string.Empty);
}