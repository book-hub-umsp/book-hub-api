using BookHub.Abstractions.Logic.Converters;
using BookHub.Models.Books;
using BookHub.Models.CRUDS.Requests;

using ContractAddAuthorBookParams = BookHub.Contracts.REST.Requests.AddAuthorBookParams;
using ContractBookParams = BookHub.Contracts.REST.Requests.BookParamsBase;
using ContractGetBookParams = BookHub.Contracts.REST.Requests.GetBookParams;
using ContractUpdateBookParams = BookHub.Contracts.REST.Requests.UpdateBookParams;
using DomainAddAuthorBookParams = BookHub.Models.CRUDS.Requests.AddAuthorBookParams;
using DomainBookParams = BookHub.Models.CRUDS.Requests.BookParamsBase;
using DomainGetBookParams = BookHub.Models.CRUDS.Requests.GetBookParams;
using DomainUpdateBookParams = BookHub.Models.CRUDS.Requests.UpdateBookParamsBase;

namespace BookHub.Logic.Converters;

/// <summary>
/// Конвертер параметров запроса к книгам.
/// </summary>
public sealed class BookParamsConverter : IBookParamsConverter
{
    public DomainBookParams Convert(ContractBookParams contractParams)
    {
        ArgumentNullException.ThrowIfNull(contractParams);

        return contractParams switch
        {
            ContractAddAuthorBookParams addBookParams =>
                addBookParams.Keywords is null
                ? new DomainAddAuthorBookParams(
                    new(addBookParams.AuthorId),
                    new(addBookParams.Genre),
                    new(addBookParams.Title),
                    new(addBookParams.Annotation))
                : new DomainAddAuthorBookParams(
                    new(addBookParams.AuthorId),
                    new(addBookParams.Genre),
                    new(addBookParams.Title),
                    new(addBookParams.Annotation),
                    addBookParams.Keywords.Select(
                        x => new KeyWord(new(x.Content))).ToList()),

            ContractGetBookParams getBookParams =>
                new DomainGetBookParams(new(getBookParams.BookId)),

            ContractUpdateBookParams updateBookParams => ConvertUpdateParams(updateBookParams),

            _ => throw new InvalidOperationException(
                $"Command type params '{contractParams.GetType().Name}' is not supported.")

        };
    }

    private static DomainUpdateBookParams ConvertUpdateParams(
        ContractUpdateBookParams contractParams)
        => contractParams switch
        {
            _ when contractParams.NewStatus is not null =>
                new UpdateBookStatusParams(
                    new(contractParams.BookId),
                    new(contractParams.AuthorId),
                    contractParams.NewStatus.Value),

            _ when contractParams.NewGenre is not null =>
                new UpdateBookGenreParams(
                    new(contractParams.BookId),
                    new(contractParams.AuthorId),
                    new(contractParams.NewGenre)),

            _ when contractParams.NewTitle is not null =>
                new UpdateBookTitleParams(
                    new(contractParams.BookId),
                    new(contractParams.AuthorId),
                    new(contractParams.NewTitle)),

            _ when contractParams.NewAnnotation is not null =>
                new UpdateBookAnnotationParams(
                    new(contractParams.BookId),
                    new(contractParams.AuthorId),
                    new(contractParams.NewAnnotation)),

            _ when contractParams.UpdatedKeywords is not null =>
                new UpdateKeyWordsParams(
                    new(contractParams.BookId),
                    new(contractParams.AuthorId),
                    contractParams.UpdatedKeywords.Select(
                        x => new KeyWord(new(x.Content))).ToList()),

            _ => throw new InvalidOperationException(
                "Received update command with empty changed parameters.")
        };
}