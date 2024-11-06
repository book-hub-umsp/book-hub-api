using BookHub.Contracts.REST.Responces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using ContractPreview = BookHub.Contracts.REST.Responces.BookPreview;

namespace BookHub.API.Controllers;

public partial class BookDescriptionController
{
    [HttpGet]
    [AllowAnonymous]
    [Route("getBy/author")]
    [ProducesResponseType<GetAllBooksPreviewsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAllAuthorBooksAsync(
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

        return Ok(content);
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("getBy/author/pagined")]
    [ProducesResponseType<GetAllPaginedBooksPreviewsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

            return Ok(content);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            _logger.LogInformation("Request was processed with failed result");

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("getBy/pagined")]
    [ProducesResponseType<GetAllPaginedBooksPreviewsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetPaginedBooksAsync(
        CancellationToken token,
        [Required][NotNull] int pageNumber = 1,
        [Required][NotNull] int elementsInPage = 5)
    {
        _logger.LogInformation("Start processing get pagined books request");

        try
        {
            var (booksPaginedPreviews, pagination) =
                await _service.GetPaginedBooksPreviewsAsync(
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

            return Ok(content);
        }
        catch (ArgumentException ex)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            _logger.LogInformation("Request was processed with failed result");

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }
}