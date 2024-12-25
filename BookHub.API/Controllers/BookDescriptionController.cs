using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using BookHub.Abstractions.Logic.Converters.Books.Repository;
using BookHub.Abstractions.Logic.Services.Books.Repository;
using BookHub.Contracts;
using BookHub.Contracts.REST.Responses;
using BookHub.Contracts.REST.Responses.Books.Repository;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ContractAddBookParams = BookHub.Contracts.REST.Requests.Books.Repository.AddBookParams;
using ContractGetBookParams = BookHub.Contracts.REST.Requests.Books.Repository.GetBookParams;
using ContractKeyWord = BookHub.Contracts.KeyWord;
using ContractPreview = BookHub.Contracts.REST.Responses.Books.Repository.BookPreview;
using ContractUpdateBookParams = BookHub.Contracts.REST.Requests.Books.Repository.UpdateBookParams;
using DomainAddBookParams = BookHub.Models.CRUDS.Requests.AddBookParams;
using DomainGetBookParams = BookHub.Models.CRUDS.Requests.GetBookParams;
using DomainUpdateBookParams = BookHub.Models.CRUDS.Requests.UpdateBookParamsBase;

namespace BookHub.API.Controllers;

/// <summary>
/// Контроллер для работы с верхнеуровневым описанием книги.
/// </summary>
[ApiController]
[Authorize]
[Route("books")]
public partial class BookDescriptionController : ControllerBase
{
    public BookDescriptionController(
        IBookDescriptionService service,
        IBookParamsConverter converter,
        ILogger<BookDescriptionController> logger)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _converter = converter ?? throw new ArgumentNullException(nameof(converter));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("{bookId}")]
    [ProducesResponseType<BookDescriptionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetBookContentAsync(
        [Required] long bookId,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogDebug(
            "Start processing book {BookId} getting request",
            bookId);

        try
        {
            var content = await _service.GetBookAsync(
                (DomainGetBookParams)_converter.Convert(
                    new ContractGetBookParams { BookId = bookId }),
                token);

            _logger.LogInformation("Request was processed with succesfull result");

            var contractContent = new BookDescriptionResponse
            {
                AuthorId = content.AuthorId.Value,
                Title = content.Description.Title.Value,
                Genre = content.Description.Genre.Value,
                Annotation = content.Description.BookAnnotation.Content,
                BookStatus = content.Status,
                CreationDate = content.CreationDate,
                LastEditTime = content.LastEditDate,
                Keywords = content.Description.KeyWords
                    .Select(x => new ContractKeyWord
                    {
                        Content = x.Content.Value
                    })
                    .ToList()
            };

            return Ok(contractContent);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("author/{authorId}")]
    [ProducesResponseType<NewsItemsResponse<ContractPreview>>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAuthorPaginatedBooksAsync(
        [Required][FromRoute] long authorId,
        [DefaultValue(1)][FromQuery] int pageNumber,
        [DefaultValue(5)][FromQuery] int elementsInPage,
        CancellationToken token)
    {
        _logger.LogDebug("Start processing get author paginated books request");

        try
        {
            var bookPreviews =
                await _service.GetAuthorBooksPreviewsAsync(
                    new(authorId),
                    new(pageNumber, elementsInPage),
                    token);

            _logger.LogInformation("Request was processed with successful result");

            return Ok(
                NewsItemsResponse<ContractPreview>.FromDomain(
                    bookPreviews,
                    ContractPreview.FromDomain));
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType<NewsItemsResponse<ContractPreview>>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetPaginatedBooksAsync(
        [DefaultValue(1)][FromQuery] int pageNumber,
        [DefaultValue(5)][FromQuery] int elementsInPage,
        CancellationToken token)
    {
        _logger.LogDebug("Start processing get paginated books request");

        try
        {
            var bookPreviews =
                await _service.GetBooksPreviewsAsync(
                    new(pageNumber, elementsInPage),
                    token);

            _logger.LogInformation("Request was processed with successful result");

            return Ok(
                NewsItemsResponse<ContractPreview>.FromDomain(
                    bookPreviews,
                    ContractPreview.FromDomain));
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType<NewsItemsResponse<ContractPreview>>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Route("keyword/{keyword}")]
    public async Task<IActionResult> GetPaginatedBooksByKeywordAsync(
        [Required][FromRoute] string keyword,
        [DefaultValue(1)][FromQuery] int pageNumber,
        [DefaultValue(5)][FromQuery] int elementsInPage,
        CancellationToken token)
    {
        _logger.LogDebug(
            "Start processing get paginated books containing keyword '{Keyword}' request",
            keyword);

        try
        {
            var bookPreviews =
                await _service.GetBooksPreviewsByKeywordAsync(
                    new(new(keyword)),
                    new(pageNumber, elementsInPage),
                    token);

            _logger.LogInformation("Request was processed with successful result");

            return Ok(
                NewsItemsResponse<ContractPreview>.FromDomain(
                    bookPreviews,
                    ContractPreview.FromDomain));
        }
        catch (ArgumentException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddBookAsync(
        [Required][NotNull] ContractAddBookParams addAuthorBookParams,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogDebug("Start processing new book adding request");

        try
        {
            await _service.AddBookAsync(
                (DomainAddBookParams)_converter.Convert(addAuthorBookParams),
                token);

            _logger.LogInformation("Request was processed with succesfull result");

            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    // Todo: will be accepted only for book author
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateBookAsync(
        [Required][NotNull] ContractUpdateBookParams updateParams,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogDebug(
            "Start processing book {BookId} updating request",
            updateParams.BookId);

        try
        {
            await _service.UpdateBookAsync(
                (DomainUpdateBookParams)_converter.Convert(updateParams),
                token);

            _logger.LogInformation("Request was processed with succesfull result");

            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    private readonly IBookDescriptionService _service;
    private readonly IBookParamsConverter _converter;
    private readonly ILogger _logger;
}