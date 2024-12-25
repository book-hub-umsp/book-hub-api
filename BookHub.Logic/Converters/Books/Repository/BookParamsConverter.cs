using BookHub.Abstractions.Logic.Converters.Books.Repository;
using BookHub.Models.Books.Repository;
using BookHub.Models.CRUDS.Requests;

using ContractAddBookParams = BookHub.Contracts.REST.Requests.Books.Repository.AddBookParams;
using ContractBookParams = BookHub.Contracts.REST.Requests.Books.Repository.BookParamsBase;
using ContractGetBookParams = BookHub.Contracts.REST.Requests.Books.Repository.GetBookParams;
using ContractUpdateBookParams = BookHub.Contracts.REST.Requests.Books.Repository.UpdateBookParams;
using DomainAddBookParams = BookHub.Models.CRUDS.Requests.AddBookParams;
using DomainBookParams = BookHub.Models.CRUDS.Requests.BookParamsBase;
using DomainGetBookParams = BookHub.Models.CRUDS.Requests.GetBookParams;
using DomainUpdateBookParams = BookHub.Models.CRUDS.Requests.UpdateBookParamsBase;

namespace BookHub.Logic.Converters.Books.Repository;

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
            ContractAddBookParams addBookParams =>
                addBookParams.Keywords is null
                ? new DomainAddBookParams(
                    new(addBookParams.Genre),
                    new(addBookParams.Title),
                    new(addBookParams.Annotation))
                : new DomainAddBookParams(
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
                    contractParams.NewStatus.Value),

            _ when contractParams.NewGenre is not null =>
                new UpdateBookGenreParams(
                    new(contractParams.BookId),
                    new(contractParams.NewGenre)),

            _ when contractParams.NewTitle is not null =>
                new UpdateBookTitleParams(
                    new(contractParams.BookId),
                    new(contractParams.NewTitle)),

            _ when contractParams.NewAnnotation is not null =>
                new UpdateBookAnnotationParams(
                    new(contractParams.BookId),
                    new(contractParams.NewAnnotation)),

            _ when contractParams.UpdatedKeywords is not null =>
                new ExtendKeyWordsParams(
                    new(contractParams.BookId),
                    contractParams.UpdatedKeywords.Select(
                        x => new KeyWord(new(x.Content))).ToList()),

            _ => throw new InvalidOperationException(
                "Received update command with empty changed parameters.")
        };
}