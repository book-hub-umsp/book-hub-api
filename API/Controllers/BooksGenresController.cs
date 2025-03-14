﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using BookHub.API.Abstractions.Logic.Services.Books.Repository;
using BookHub.API.Contracts;
using BookHub.API.Contracts.REST.Requests.Books.Repository;
using BookHub.API.Contracts.REST.Responses.Books.Repository;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookHub.API.Service.Controllers;

/// <summary>
/// Контроллер для работы с жанрами книг.
/// </summary>
[ApiController]
[Authorize]
[Route("genres")]
public sealed class BooksGenresController : ControllerBase
{
    public BooksGenresController(
        IBookGenresService service,
        ILogger<BooksGenresController> logger)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AddBookGenreAsync(
        [Required][FromQuery][NotNull] string newGenre,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        try
        {
            await _service.AddBookGenreAsync(new(newGenre), token);

            _logger.LogInformation("Request was processed with successfully result");

            return Ok();
        }
        catch (Exception ex)
        when (ex is InvalidOperationException or ArgumentException)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType<BooksGenresListResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<BooksGenresListResponse>> GetBooksGenresAsync(
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        try
        {
            var genres = await _service.GetBooksGenresAsync(token);

            _logger.LogInformation("Request was processed with successfully result");

            var contractContent = BooksGenresListResponse.FromDomain(genres);

            return Ok(contractContent);
        }
        catch (Exception ex)
        when (ex is InvalidOperationException or ArgumentException)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<FailureCommandResultResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteBookGenreAsync(
        [FromRoute][Required][NotNull] long id,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        try
        {
            await _service.RemoveBookGenreAsync(new(id), token);

            _logger.LogInformation("Request was processed with successfully result");

            return Ok();
        }
        catch (Exception ex)
        when (ex is InvalidOperationException or ArgumentException)
        {
            _logger.LogError("Error is happened: '{Message}'", ex.Message);

            return BadRequest(FailureCommandResultResponse.FromException(ex));
        }
    }

    private readonly IBookGenresService _service;
    private readonly ILogger<BooksGenresController> _logger;
}