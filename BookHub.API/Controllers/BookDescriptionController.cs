﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using BookHub.Abstractions.Logic.Converters;
using BookHub.Abstractions.Logic.Services;
using BookHub.Contracts.REST.Responces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ContractAddAuthorBookParams = BookHub.Contracts.REST.Requests.AddAuthorBookParams;
using ContractGetBookParams = BookHub.Contracts.REST.Requests.GetBookParams;
using ContractKeyWord = BookHub.Contracts.KeyWord;
using ContractUpdateBookParams = BookHub.Contracts.REST.Requests.UpdateBookParams;
using DomainAddAuthorBookParams = BookHub.Models.CRUDS.Requests.AddAuthorBookParams;
using DomainGetBookParams = BookHub.Models.CRUDS.Requests.GetBookParams;
using DomainUpdateBookParams = BookHub.Models.CRUDS.Requests.UpdateBookParamsBase;
using ContractPreview = BookHub.Contracts.REST.Responces.BookPreview;
using BookHub.Contracts.REST.Responces.Users;

namespace BookHub.API.Controllers;

/// <summary>
/// Контроллер для работы с верхнеуровневым описанием книги.
/// </summary>
[ApiController]
[Authorize]
[Route("books")]
public class BookDescriptionController : ControllerBase
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

    [HttpPost]
    [Route("add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddNewAuthorBookAsync(
        [Required][NotNull] ContractAddAuthorBookParams addAuthorBookParams,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogInformation(
            "Start processing new book adding request for author {AuthorId}",
            addAuthorBookParams.AuthorId);

        try
        {
            await _service.AddBookAsync(
                (DomainAddAuthorBookParams)_converter.Convert(addAuthorBookParams),
                token);

            _logger.LogInformation("Request was processed with succesfull result");

            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            _logger.LogInformation("Request was processed with failed result");

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("get/{bookId}")]
    [ProducesResponseType<BookDescriptionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetBookContentAsync(
        [Required][NotNull] long bookId,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogInformation(
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

            return Ok(new SuccesfullCommandResultResponse { Content = contractContent });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            _logger.LogInformation("Request was processed with failed result");

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("get")]
    [ProducesResponseType<GetAllBooksPreviewsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAuthorBooksAsync(
        [Required][NotNull] long authorId,
        CancellationToken token)
    {
        _logger.LogInformation("Start processing get author books request");

        var booksPreviews = await _service.GetAuthorBooksPreviewsAsync(
            new(authorId), 
            token);

        var content = new GetAllBooksPreviewsResponse
        {
            Previews = booksPreviews.Select(
                x => new ContractPreview
                {
                    Id = x.Id.Value,
                    AuthorId = x.AuthorId.Value,
                    Genre = x.Genre.Value,
                    Title = x.Title.Value
                }).ToList()
        };

        _logger.LogInformation("Request was processed with succesfull result");

        return Ok(new SuccesfullCommandResultResponse { Content = content });
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("getPagined")]
    [ProducesResponseType<GetAllPaginedBooksPreviewsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    // /books/getPagined?authorId=1&pageNumber=x&elementsInPage=y
    public async Task<IActionResult> GetAuthorPaginedBooksAsync(
        [Required][NotNull] long authorId,
        CancellationToken token,
        [Required][NotNull] int pageNumber = 1,
        [Required][NotNull] int elementsInPage = 5)
    {
        _logger.LogInformation("Start processing get author pagined books request");

        try
        {
            var (booksPaginedPreviews, pagination) =
                await _service.GetAuthorPaginedBooksPreviewsAsync(
                    new(authorId), 
                    new(pageNumber, elementsInPage), 
                    token);

            var content = new GetAllPaginedBooksPreviewsResponse
            {
                ElementsTotal = pagination.ElementsTotal,

                PagesTotal = pagination.PagesTotal,

                Previews = booksPaginedPreviews.Select(
                    x => new ContractPreview
                    {
                        Id = x.Id.Value,
                        AuthorId = x.AuthorId.Value,
                        Genre = x.Genre.Value,
                        Title = x.Title.Value
                    }).ToList()
            };

            _logger.LogInformation("Request was processed with succesfull result");

            return Ok(new SuccesfullCommandResultResponse { Content = content });
        }
        catch (ArgumentException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            _logger.LogInformation("Request was processed with failed result");

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    [HttpPut]
    [Route("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateBookAsync(
        [Required][NotNull] ContractUpdateBookParams updateParams,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _logger.LogInformation(
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

            _logger.LogInformation("Request was processed with failed result");

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    private readonly IBookDescriptionService _service;
    private readonly IBookParamsConverter _converter;
    private readonly ILogger _logger;
}