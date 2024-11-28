using BookHub.Abstractions.Logic.Services.Books.Content;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookHub.API.Controllers;

[ApiController]
[Authorize]
[Route("chapters")]
public sealed class ChaptersController : ControllerBase
{
    public ChaptersController(
        IChaptersService chaptersService,
        ILogger<ChaptersController> logger)
    {
        _chaptersService = chaptersService
            ?? throw new ArgumentNullException(nameof(chaptersService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private readonly IChaptersService _chaptersService;
    private readonly ILogger<ChaptersController> _logger;
}